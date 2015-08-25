using System;
using System.Collections.Generic;
using System.Linq;
using Llvm.NET.Values;
using Llvm.NET.Instructions;

namespace Llvm.NET.DebugInfo
{
    /// <summary>DebugInfoBuilder is a factory class for creating DebugInformation for an LLVM <see cref="Module"/></summary>
    /// <remarks>
    /// Many Debug information metadata nodes are created with unresolved references to additional metadata. To ensure such
    /// metadata is resolved applications should call the <see cref="Finish"/> method to resolve and finalize the metadata.
    /// After this point only fully resolved nodes may be added to ensure that the data remains valid.
    /// </remarks>
    public sealed class DebugInfoBuilder : IDisposable
    {
        public DebugInfoBuilder( Module owningModule )
            : this( owningModule, true )
        {
        }

        // keeping this private for now as there doesn't seem to be a good reason to support
        // allowUnresolved == false
        private DebugInfoBuilder( Module owningModule, bool allowUnresolved )
        {
            BuilderHandle = LLVMNative.NewDIBuilder( owningModule.ModuleHandle, allowUnresolved );
        }

        public DiCompileUnit CreateCompileUnit( SourceLanguage language
                                            , string fileName
                                            , string filePath
                                            , string producer
                                            , bool optimized
                                            , string flags
                                            , uint runtimeVersion
                                            )
        {
            var handle = LLVMNative.DIBuilderCreateCompileUnit( BuilderHandle
                                                              , ( uint )language
                                                              , fileName
                                                              , filePath
                                                              , producer
                                                              , optimized ? 1 : 0
                                                              , flags
                                                              , runtimeVersion
                                                              );
            return new DiCompileUnit( handle );
        }

        public DiFile CreateFile( string path )
        {
            if( string.IsNullOrWhiteSpace( path ) )
                throw new ArgumentException( "Path cannot be null, empty or whitespace" );

            return CreateFile( System.IO.Path.GetFileName( path ), System.IO.Path.GetDirectoryName( path ) );
        }

        public DiFile CreateFile( string fileName, string directory )
        {
            if( string.IsNullOrWhiteSpace( fileName ) )
                throw new ArgumentException( "File name cannot be empty or null" );

            var handle = LLVMNative.DIBuilderCreateFile( BuilderHandle, fileName, directory??string.Empty );
            // REVIEW: should this deal with uniquing? if so, is it per context? Per module? ...?
            return new DiFile( handle );
        }

        public DiLexicalBlock CreateLexicalBlock( DiScope scope, DiFile file, uint line, uint column )
        {
            var handle = LLVMNative.DIBuilderCreateLexicalBlock( BuilderHandle, scope.MetadataHandle, file.MetadataHandle, line, column );
            return new DiLexicalBlock( handle );
        }

        public DiLexicalBlockFile CreateLexicalBlockFile( DiScope scope, DiFile file, uint discriminator )
        {
            var handle = LLVMNative.DIBuilderCreateLexicalBlockFile( BuilderHandle, scope.MetadataHandle, file.MetadataHandle, discriminator );
            return new DiLexicalBlockFile( handle );
        }

        public DiSubProgram CreateFunction( DiScope scope
                                        , string name
                                        , string mangledName
                                        , DiFile file
                                        , uint line
                                        , DiCompositeType compositeType
                                        , bool isLocalToUnit
                                        , bool isDefinition
                                        , uint scopeLine
                                        , uint flags
                                        , bool isOptimized
                                        , Function function
                                        )
        {
            if( string.IsNullOrWhiteSpace( name ) )
                name = string.Empty;

            if( string.IsNullOrWhiteSpace( mangledName ) )
                mangledName = string.Empty;

            var handle = LLVMNative.DIBuilderCreateFunction( BuilderHandle
                                                           , scope.MetadataHandle
                                                           , name
                                                           , mangledName
                                                           , file.MetadataHandle
                                                           , line
                                                           , compositeType.MetadataHandle
                                                           , isLocalToUnit ? 1 : 0
                                                           , isDefinition ? 1 : 0
                                                           , scopeLine
                                                           , flags
                                                           , isOptimized ? 1 : 0
                                                           , function.ValueHandle
                                                           );
            return new DiSubProgram( handle );
        }

        public DiLocalVariable CreateLocalVariable( uint dwarfTag
                                                , DiScope scope
                                                , string name
                                                , DiFile file
                                                , uint line
                                                , DiType type
                                                , bool alwaysPreserve
                                                , uint flags
                                                , uint argNo
                                                )
        {
            var handle = LLVMNative.DIBuilderCreateLocalVariable( BuilderHandle
                                                                , dwarfTag
                                                                , scope.MetadataHandle
                                                                , name
                                                                , file.MetadataHandle
                                                                , line
                                                                , type.MetadataHandle
                                                                , alwaysPreserve ? 1 : 0
                                                                , flags
                                                                , argNo
                                                                );
            return new DiLocalVariable( handle );
        }

        public DiBasicType CreateBasicType( string name, ulong bitSize, ulong bitAlign, DiTypeKind encoding )
        {
            var handle = LLVMNative.DIBuilderCreateBasicType( BuilderHandle, name, bitSize, bitAlign, (uint)encoding );
            return new DiBasicType( handle );
        }

        public DiDerivedType CreatePointerType( DiType pointeeType, string name, ulong bitSize, ulong bitAlign)
        {
            var handle = LLVMNative.DIBuilderCreatePointerType( BuilderHandle, pointeeType.MetadataHandle, bitSize, bitAlign, name ?? string.Empty );
            return new DiDerivedType( handle );
        }

        public DiTypeArray CreateTypeArray( params DiType[ ] types )
        {
            var handles = types.Select( t => t.MetadataHandle ).ToArray( );
            var count = handles.LongLength;
            if( count == 0 )
                handles = new LLVMMetadataRef[ ] { default( LLVMMetadataRef ) };

            var handle = LLVMNative.DIBuilderGetOrCreateTypeArray( BuilderHandle, out handles[ 0 ], (ulong)count );
            return new DiTypeArray( handle );
        }

        public DiSubroutineType CreateSubroutineType( DiFile file, DiTypeArray types )
        {
            var handle = LLVMNative.DIBuilderCreateSubroutineType( BuilderHandle, file.MetadataHandle, types.MetadataHandle );
            return new DiSubroutineType( handle );
        }

        public DiSubroutineType CreateSubroutineType( DiFile file, params DiType[] types )
        {
            var typeArray = GetOrCreateTypeArray( types );
            return CreateSubroutineType( file, typeArray );
        }

        public DiCompositeType CreateStructType( DiScope scope
                                             , string name
                                             , DiFile file
                                             , uint line
                                             , ulong bitSize
                                             , ulong bitAlign
                                             , uint flags
                                             , DiType derivedFrom
                                             , DiArray elements
                                             )
        {
            var handle = LLVMNative.DIBuilderCreateStructType( BuilderHandle
                                                             , scope.MetadataHandle
                                                             , name
                                                             , file?.MetadataHandle ?? LLVMMetadataRef.Zero
                                                             , line
                                                             , bitSize
                                                             , bitAlign
                                                             , flags
                                                             , derivedFrom?.MetadataHandle ?? LLVMMetadataRef.Zero
                                                             , elements.MetadataHandle
                                                             );
            return new DiCompositeType( handle );
        }

        public DiCompositeType CreateStructType( DiScope scope
                                             , string name
                                             , DiFile file
                                             , uint line
                                             , ulong bitSize
                                             , ulong bitAlign
                                             , uint flags
                                             , DiType derivedFrom
                                             , params DiDescriptor[] elements
                                             )
        {
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, flags, derivedFrom, GetOrCreateArray( elements ) );
        }

        public DiCompositeType CreateStructType( DiScope scope
                                             , string name
                                             , DiFile file
                                             , uint line
                                             , ulong bitSize
                                             , ulong bitAlign
                                             , uint flags
                                             , DiType derivedFrom
                                             , IEnumerable<DiDescriptor> elements
                                             )
        {
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, flags, derivedFrom, GetOrCreateArray( elements ) );
        }

        public DiDerivedType CreateMemberType( DiScope scope
                                           , string name
                                           , DiFile file
                                           , uint line
                                           , ulong bitSize
                                           , ulong bitAlign
                                           , ulong bitOffset
                                           , uint flags
                                           , DiType type
                                           )
        {
            var handle = LLVMNative.DIBuilderCreateMemberType( BuilderHandle
                                                             , scope.MetadataHandle
                                                             , name
                                                             , file?.MetadataHandle ?? LLVMMetadataRef.Zero
                                                             , line
                                                             , bitSize
                                                             , bitAlign
                                                             , bitOffset
                                                             , flags
                                                             , type.MetadataHandle
                                                             );
            return new DiDerivedType( handle );
        }

        public DiCompositeType CreateArrayType( ulong bitSize, ulong bitAlign, DiType elementType, DiArray subScripts )
        {
            var handle = LLVMNative.DIBuilderCreateArrayType( BuilderHandle, bitSize, bitAlign, elementType.MetadataHandle, subScripts.MetadataHandle );
            return new DiCompositeType( handle );
        }

        public DiCompositeType CreateArrayType( ulong bitSize, ulong bitAlign, DiType elementType, params DiDescriptor[] subScripts )
        {
            return CreateArrayType( bitSize, bitAlign, elementType, GetOrCreateArray( subScripts ) );
        }

        public DiDerivedType CreateTypedef(DiType type, string name, DiFile file, uint line, DiDescriptor context )
        {
            var handle = LLVMNative.DIBuilderCreateTypedef( BuilderHandle, type.MetadataHandle, name, file.MetadataHandle, line, context.MetadataHandle );
            return new DiDerivedType( handle );
        }

        public DiSubrange CreateSubrange( long lo, long count )
        {
            var handle = LLVMNative.DIBuilderGetOrCreateSubrange( BuilderHandle, lo, count );
            return new DiSubrange( handle );
        }

        public DiArray GetOrCreateArray( IEnumerable<DiDescriptor> elements )
        {
            var buf = elements.Select( d => d?.MetadataHandle ?? LLVMMetadataRef.Zero ).ToArray( );
            var actualLen = buf.LongLength;
            // for the out parameter trick to work - need to have a valid array with at least one element
            if( buf.LongLength == 0 )
                buf = new LLVMMetadataRef[ 1 ];

            var handle = LLVMNative.DIBuilderGetOrCreateArray( BuilderHandle, out buf[ 0 ], ( ulong )actualLen );
            return new DiArray( handle );
        }

        public DiTypeArray GetOrCreateTypeArray( IEnumerable<DiType> types )
        {
            var buf = types.Select( t => t?.MetadataHandle ?? LLVMMetadataRef.Zero ).ToArray();
            var handle = LLVMNative.DIBuilderGetOrCreateTypeArray( BuilderHandle, out buf[ 0 ], (ulong)buf.LongLength );
            return new DiTypeArray( handle );
        }

        public DiEnumerator CreateEnumeratorValue( string name, long value )
        {
            var handle = LLVMNative.DIBuilderCreateEnumeratorValue( BuilderHandle, name, value );
            return new DiEnumerator( handle );
        }

        public DiCompositeType CreateEnumerationType( DiScope scope
                                                  , string name
                                                  , DiFile file
                                                  , uint lineNumber
                                                  , ulong sizeInBits
                                                  , ulong alignInBits
                                                  , IEnumerable<DiEnumerator> elements
                                                  , DiType underlyingType
                                                  , string uniqueId = ""
                                                  )
        {
            var elementHandles = elements.Select( e => e.MetadataHandle ).ToArray( );
            var elementArray = LLVMNative.DIBuilderGetOrCreateArray( BuilderHandle, out elementHandles[ 0 ], (ulong)elementHandles.LongLength );
            var handle = LLVMNative.DIBuilderCreateEnumerationType( BuilderHandle
                                                                  , scope.MetadataHandle
                                                                  , name
                                                                  , file.MetadataHandle
                                                                  , lineNumber
                                                                  , sizeInBits
                                                                  , alignInBits
                                                                  , elementArray
                                                                  , underlyingType.MetadataHandle
                                                                  , uniqueId
                                                                  );
            return new DiCompositeType( handle );
        }

        public DiGlobalVariable CreateGlobalVariable( DiDescriptor scope
                                                  , string name
                                                  , string linkageName
                                                  , DiFile file
                                                  , uint lineNo
                                                  , DiType type
                                                  , bool isLocalToUnit
                                                  , Value value
                                                  , DiDescriptor decl
                                                  )
        {
            var handle = LLVMNative.DIBuilderCreateGlobalVariable( BuilderHandle
                                                                 , scope.MetadataHandle
                                                                 , name
                                                                 , linkageName
                                                                 , file.MetadataHandle
                                                                 , lineNo
                                                                 , type.MetadataHandle
                                                                 , isLocalToUnit
                                                                 , value.ValueHandle
                                                                 , decl.MetadataHandle
                                                                 );
            return new DiGlobalVariable( handle );
        }

        public void Finish()
        {
            if( !IsFinished )
            {
                LLVMNative.DIBuilderFinalize( BuilderHandle );
                IsFinished = true;
            }
        }

        public Value InsertDeclare( Value storage, DiLocalVariable varInfo, Instruction insertBefore )
        {
            return InsertDeclare( storage, varInfo, CreateExpression( ), insertBefore );
        }

        public Value InsertDeclare( Value storage, DiLocalVariable varInfo, DiExpression expr, Instruction insertBefore )
        {
            var handle = LLVMNative.DIBuilderInsertDeclareBefore( BuilderHandle
                                                                , storage.ValueHandle
                                                                , varInfo.MetadataHandle
                                                                , expr.MetadataHandle
                                                                , insertBefore.ValueHandle
                                                                );
            return Value.FromHandle( handle );
        }

        public Value InsertDeclare( Value storage, DiLocalVariable varInfo, BasicBlock insertAtEnd )
        {
            return InsertDeclare( storage, varInfo, CreateExpression( ), insertAtEnd );
        }

        public Value InsertDeclare( Value storage, DiLocalVariable varInfo, DiExpression expr, BasicBlock insertAtEnd )
        {
            var handle = LLVMNative.DIBuilderInsertDeclareAtEnd( BuilderHandle
                                                                , storage.ValueHandle
                                                                , varInfo.MetadataHandle
                                                                , expr.MetadataHandle
                                                                , insertAtEnd.BlockHandle
                                                                );
            return Value.FromHandle( handle );
        }

        public DiExpression CreateExpression( params ExpressionOp[ ] operations ) => CreateExpression( ( IEnumerable<ExpressionOp> )operations );

        public DiExpression CreateExpression( IEnumerable<ExpressionOp> operations )
        {
            var args = operations.Cast<long>().ToArray( );
            var actualCount = args.LongLength;
            if( args.Length == 0 )
                args = new long[ 1 ];

            var handle = LLVMNative.DIBuilderCreateExpression( BuilderHandle, out args[ 0 ], (ulong)actualCount );
            return new DiExpression( handle );
        }

        public DiCompositeType CreateReplaceableForwardDecl( Tag tag
                                                         , string name
                                                         , DiDescriptor scope
                                                         , DiFile file
                                                         , uint line
                                                         , uint lang = 0
                                                         , ulong sizeInBits = 0
                                                         , ulong alignBits = 0
                                                         , string uniqueIdentifier = null
                                                         )
        {
            var handle = LLVMNative.DIBuilderCreateReplaceableForwardDecl( BuilderHandle
                                                                         , ( uint )tag
                                                                         , name
                                                                         , scope.MetadataHandle
                                                                         , file?.MetadataHandle ?? LLVMMetadataRef.Zero
                                                                         , line
                                                                         , lang
                                                                         , sizeInBits
                                                                         , alignBits
                                                                         , uniqueIdentifier ?? string.Empty
                                                                         );
            return new DiCompositeType( handle );
        }

        public void Dispose( )
        {
            if( BuilderHandle.Pointer != IntPtr.Zero )
            {
                LLVMNative.DIBuilderDestroy( BuilderHandle );
                BuilderHandle = default(LLVMDIBuilderRef);
            }
        }

        private bool IsFinished;
        internal LLVMDIBuilderRef BuilderHandle { get; private set; }
    }
}
