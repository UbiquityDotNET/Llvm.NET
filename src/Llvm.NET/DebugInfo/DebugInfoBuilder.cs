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

        public CompileUnit CreateCompileUnit( SourceLanguage language
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
            return new CompileUnit( handle );
        }

        public File CreateFile( string path )
        {
            if( string.IsNullOrWhiteSpace( path ) )
                return File.Empty;

            return CreateFile( System.IO.Path.GetFileName( path ), System.IO.Path.GetDirectoryName( path ) );
        }

        public File CreateFile( string fileName, string directory )
        {
            if( string.IsNullOrWhiteSpace( fileName ) )
                throw new ArgumentException( "File name cannot be empty or null" );

            var handle = LLVMNative.DIBuilderCreateFile( BuilderHandle, fileName, directory??string.Empty );
            // REVIEW: should this deal with uniquing? if so, is it per context? Per module? ...?
            return new File( handle );
        }

        public LexicalBlock CreateLexicalBlock( Scope scope, File file, uint line, uint column )
        {
            var handle = LLVMNative.DIBuilderCreateLexicalBlock( BuilderHandle, scope.MetadataHandle, file.MetadataHandle, line, column );
            return new LexicalBlock( handle );
        }

        public LexicalBlockFile CreateLexicalBlockFile( Scope scope, File file, uint discriminator )
        {
            var handle = LLVMNative.DIBuilderCreateLexicalBlockFile( BuilderHandle, scope.MetadataHandle, file.MetadataHandle, discriminator );
            return new LexicalBlockFile( handle );
        }

        public SubProgram CreateFunction( Scope scope
                                        , string name
                                        , string mangledName
                                        , File file
                                        , uint line
                                        , CompositeType compositeType
                                        , bool isLocalToUnit
                                        , bool isDefinition
                                        , uint scopeLine
                                        , uint flags
                                        , bool isOptimized
                                        , Function function
                                        )
        {
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
            return new SubProgram( handle );
        }

        public LocalVariable CreateLocalVariable( uint dwarfTag
                                                , Scope scope
                                                , string name
                                                , File file
                                                , uint line
                                                , Type type
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
            return new LocalVariable( handle );
        }

        public BasicType CreateBasicType( string name, ulong bitSize, ulong bitAlign, TypeKind encoding )
        {
            var handle = LLVMNative.DIBuilderCreateBasicType( BuilderHandle, name, bitSize, bitAlign, (uint)encoding );
            return new BasicType( handle );
        }

        public DerivedType CreatePointerType( Type pointeeType, string name, ulong bitSize, ulong bitAlign)
        {
            var handle = LLVMNative.DIBuilderCreatePointerType( BuilderHandle, pointeeType.MetadataHandle, bitSize, bitAlign, name );
            return new DerivedType( handle );
        }

        public TypeArray CreateTypeArray( params Type[ ] types )
        {
            var handles = types.Select( t => t.MetadataHandle ).ToArray( );
            var count = handles.LongLength;
            if( count == 0 )
                handles = new LLVMMetadataRef[ ] { default( LLVMMetadataRef ) };

            var handle = LLVMNative.DIBuilderGetOrCreateTypeArray( BuilderHandle, out handles[ 0 ], (ulong)count );
            return new TypeArray( handle );
        }

        public SubroutineType CreateSubroutineType( File file, TypeArray types )
        {
            var handle = LLVMNative.DIBuilderCreateSubroutineType( BuilderHandle, file.MetadataHandle, types.MetadataHandle );
            return new SubroutineType( handle );
        }

        public SubroutineType CreateSubroutineType( File file, params Type[] types )
        {
            var typeArray = GetOrCreateTypeArray( types );
            return CreateSubroutineType( file, typeArray );
        }

        public CompositeType CreateStructType( Scope scope
                                             , string name
                                             , File file
                                             , uint line
                                             , ulong bitSize
                                             , ulong bitAlign
                                             , uint flags
                                             , Type derivedFrom
                                             , Array elements
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
            return new CompositeType( handle );
        }

        public CompositeType CreateStructType( Scope scope
                                             , string name
                                             , File file
                                             , uint line
                                             , ulong bitSize
                                             , ulong bitAlign
                                             , uint flags
                                             , Type derivedFrom
                                             , params Descriptor[] elements
                                             )
        {
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, flags, derivedFrom, GetOrCreateArray( elements ) );
        }

        public CompositeType CreateStructType( Scope scope
                                             , string name
                                             , File file
                                             , uint line
                                             , ulong bitSize
                                             , ulong bitAlign
                                             , uint flags
                                             , Type derivedFrom
                                             , IEnumerable<Descriptor> elements
                                             )
        {
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, flags, derivedFrom, GetOrCreateArray( elements ) );
        }

        public DerivedType CreateMemberType( Scope scope
                                           , string name
                                           , File file
                                           , uint line
                                           , ulong bitSize
                                           , ulong bitAlign
                                           , ulong bitOffset
                                           , uint flags
                                           , Type type
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
            return new DerivedType( handle );
        }

        public CompositeType CreateArrayType( ulong bitSize, ulong bitAlign, Type elementType, Array subScripts )
        {
            var handle = LLVMNative.DIBuilderCreateArrayType( BuilderHandle, bitSize, bitAlign, elementType.MetadataHandle, subScripts.MetadataHandle );
            return new CompositeType( handle );
        }

        public CompositeType CreateArrayType( ulong bitSize, ulong bitAlign, Type elementType, params Descriptor[] subScripts )
        {
            return CreateArrayType( bitSize, bitAlign, elementType, GetOrCreateArray( subScripts ) );
        }

        public DerivedType CreateTypedef(Type type, string name, File file, uint line, Descriptor context )
        {
            var handle = LLVMNative.DIBuilderCreateTypedef( BuilderHandle, type.MetadataHandle, name, file.MetadataHandle, line, context.MetadataHandle );
            return new DerivedType( handle );
        }

        public Subrange CreateSubrange( long lo, long count )
        {
            var handle = LLVMNative.DIBuilderGetOrCreateSubrange( BuilderHandle, lo, count );
            return new Subrange( handle );
        }

        public Array GetOrCreateArray( IEnumerable<Descriptor> elements )
        {
            var buf = elements.Select( d => d.MetadataHandle ).ToArray( );
            var actualLen = buf.LongLength;
            // for the out parameter trick to work - need to have a valid array with at least one element
            if( buf.LongLength == 0 )
                buf = new LLVMMetadataRef[ 1 ];

            var handle = LLVMNative.DIBuilderGetOrCreateArray( BuilderHandle, out buf[ 0 ], ( ulong )actualLen );
            return new Array( handle );
        }

        public TypeArray GetOrCreateTypeArray( IEnumerable<Type> types )
        {
            var buf = types.Select( t => t.MetadataHandle ).ToArray();
            var handle = LLVMNative.DIBuilderGetOrCreateTypeArray( BuilderHandle, out buf[ 0 ], (ulong)buf.LongLength );
            return new TypeArray( handle );
        }

        public Enumerator CreateEnumeratorValue( string name, long value )
        {
            var handle = LLVMNative.DIBuilderCreateEnumeratorValue( BuilderHandle, name, value );
            return new Enumerator( handle );
        }

        public CompositeType CreateEnumerationType( Scope scope
                                                  , string name
                                                  , File file
                                                  , uint lineNumber
                                                  , ulong sizeInBits
                                                  , ulong alignInBits
                                                  , IEnumerable<Enumerator> elements
                                                  , Type underlyingType
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
            return new CompositeType( handle );
        }

        public GlobalVariable CreateGlobalVariable( Descriptor scope
                                                  , string name
                                                  , string linkageName
                                                  , File file
                                                  , uint lineNo
                                                  , Type type
                                                  , bool isLocalToUnit
                                                  , Value value
                                                  , Descriptor decl
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
            return new GlobalVariable( handle );
        }

        public void Finish()
        {
            if( !IsFinished )
            {
                LLVMNative.DIBuilderFinalize( BuilderHandle );
                IsFinished = true;
            }
        }

        public Value InsertDeclare( Value storage, LocalVariable varInfo, Instruction insertBefore )
        {
            return InsertDeclare( storage, varInfo, CreateExpression( ), insertBefore );
        }

        public Value InsertDeclare( Value storage, LocalVariable varInfo, Expression expr, Instruction insertBefore )
        {
            var handle = LLVMNative.DIBuilderInsertDeclareBefore( BuilderHandle
                                                                , storage.ValueHandle
                                                                , varInfo.MetadataHandle
                                                                , expr.MetadataHandle
                                                                , insertBefore.ValueHandle
                                                                );
            return Value.FromHandle( handle );
        }

        public Value InsertDeclare( Value storage, LocalVariable varInfo, BasicBlock insertAtEnd )
        {
            return InsertDeclare( storage, varInfo, CreateExpression( ), insertAtEnd );
        }

        public Value InsertDeclare( Value storage, LocalVariable varInfo, Expression expr, BasicBlock insertAtEnd )
        {
            var handle = LLVMNative.DIBuilderInsertDeclareAtEnd( BuilderHandle
                                                                , storage.ValueHandle
                                                                , varInfo.MetadataHandle
                                                                , expr.MetadataHandle
                                                                , insertAtEnd.BlockHandle
                                                                );
            return Value.FromHandle( handle );
        }

        public Expression CreateExpression( params ExpressionOp[ ] operations ) => CreateExpression( ( IEnumerable<ExpressionOp> )operations );

        public Expression CreateExpression( IEnumerable<ExpressionOp> operations )
        {
            var args = operations.Cast<long>().ToArray( );
            var actualCount = args.LongLength;
            if( args.Length == 0 )
                args = new long[ 1 ];

            var handle = LLVMNative.DIBuilderCreateExpression( BuilderHandle, out args[ 0 ], (ulong)actualCount );
            return new Expression( handle );
        }

        public CompositeType CreateReplaceableForwardDecl( Tag tag
                                                         , string name
                                                         , Descriptor scope
                                                         , File file
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
                                                                         , file.MetadataHandle
                                                                         , line
                                                                         , lang
                                                                         , sizeInBits
                                                                         , alignBits
                                                                         , uniqueIdentifier ?? string.Empty
                                                                         );
            return new CompositeType( handle );
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
