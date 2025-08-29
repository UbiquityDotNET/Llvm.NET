// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>DIBuilder is a factory class for creating DebugInformation for an LLVM <see cref="Module"/></summary>
    /// <remarks>
    /// <para>Many Debug information metadata nodes are created with unresolved references to additional
    /// metadata. To ensure such metadata is resolved applications should call the <see cref="Finish()"/>
    /// method to resolve and finalize the metadata. After this point only fully resolved nodes may
    /// be added to ensure that the data remains valid.</para>
    /// <para>This type is a class to allow storing it as a member anywhere. It is NOT a member of
    /// <see cref="Module"/> but has one associated with it. Generally, at most one <see cref="DICompileUnit"/>
    /// is associated with a <see cref="DIBuilder"/>. If creating a function
    /// (via <see cref="CreateFunction(DIScope?, LazyEncodedString, LazyEncodedString, DIFile?, uint, DISubroutineType?, bool, bool, uint, DebugInfoFlags, bool, Function)"/>),
    /// then the creation of a <see cref="DICompileUnit"/> is required.</para>
    /// <para>
    /// As this type implements <see cref="IDisposable"/> it OWNS the underlying native resource and callers must call <see cref="Dispose"/>
    /// to free the underlying native resource.
    /// </para>
    /// </remarks>
    /// <seealso href="xref:llvm_sourceleveldebugging">LLVM Source Level Debugging</seealso>
    public sealed class DIBuilder
        : IDIBuilder
        , IDisposable
        , IGlobalHandleOwner<LLVMDIBuilderRef>
        , IEquatable<DIBuilder>
    {
        /// <summary>Initializes a new instance of the <see cref="DIBuilder"/> class.</summary>
        /// <param name="owningModule">Module that owns this builder</param>
        public DIBuilder( IModule owningModule )
            : this( owningModule, true )
        {
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            NativeHandle.Dispose();
        }

        #region IEquatable<T>

        /// <inheritdoc/>
        public bool Equals( IDIBuilder? other ) => other is not null && ((LLVMDIBuilderRefAlias)NativeHandle).Equals( other.GetUnownedHandle() );

        /// <inheritdoc/>
        public bool Equals( DIBuilder? other ) => other is not null && NativeHandle.Equals( other.NativeHandle );

        /// <inheritdoc/>
        public override bool Equals( object? obj ) => obj is Context owner
                                                  ? Equals( owner )
                                                  : Equals( obj as IContext );

        /// <inheritdoc/>
        public override int GetHashCode( ) => NativeHandle.GetHashCode();

        #endregion

        /// <summary>Gets an alias interface for this instance</summary>
        /// <returns>Alias interface for this instance</returns>
        public IDIBuilder AsAlias()
        {
            return Impl;
        }

        #region IDIBuilder (via Impl)

        /// <inheritdoc/>
        public IModule OwningModule => Impl.OwningModule;

        /// <inheritdoc/>
        public DICompileUnit? CompileUnit => Impl.CompileUnit;

        /// <inheritdoc/>
        public DICompileUnit CreateCompileUnit( SourceLanguage language, string sourceFilePath, LazyEncodedString? producer, bool optimized = false, LazyEncodedString? compilationFlags = null, uint runtimeVersion = 0 )
        {
            return Impl.CreateCompileUnit( language, sourceFilePath, producer, optimized, compilationFlags, runtimeVersion );
        }

        /// <inheritdoc/>
        public DICompileUnit CreateCompileUnit( SourceLanguage language, LazyEncodedString fileName, LazyEncodedString fileDirectory, LazyEncodedString? producer, bool optimized, LazyEncodedString? compilationFlags, uint runtimeVersion, LazyEncodedString? sysRoot = null, LazyEncodedString? sdk = null )
        {
            return Impl.CreateCompileUnit( language, fileName, fileDirectory, producer, optimized, compilationFlags, runtimeVersion, sysRoot, sdk );
        }

        /// <inheritdoc/>
        public DIMacroFile CreateTempMacroFile( DIMacroFile? parent, uint line, DIFile? file )
        {
            return Impl.CreateTempMacroFile( parent, line, file );
        }

        /// <inheritdoc/>
        public DIMacro CreateMacro( DIMacroFile? parentFile, uint line, MacroKind kind, LazyEncodedString name, LazyEncodedString value )
        {
            return Impl.CreateMacro( parentFile, line, kind, name, value );
        }

        /// <inheritdoc/>
        public DINamespace CreateNamespace( DIScope? scope, LazyEncodedString name, bool exportSymbols )
        {
            return Impl.CreateNamespace( scope, name, exportSymbols );
        }

        /// <inheritdoc/>
        public DIFile CreateFile( string? path )
        {
            return Impl.CreateFile( path );
        }

        /// <inheritdoc/>
        public DIFile CreateFile( LazyEncodedString? fileName, LazyEncodedString? directory )
        {
            return Impl.CreateFile( fileName, directory );
        }

        /// <inheritdoc/>
        public DILexicalBlock CreateLexicalBlock( DIScope? scope, DIFile? file, uint line, uint column )
        {
            return Impl.CreateLexicalBlock( scope, file, line, column );
        }

        /// <inheritdoc/>
        public DILexicalBlockFile CreateLexicalBlockFile( DIScope? scope, DIFile? file, uint discriminator )
        {
            return Impl.CreateLexicalBlockFile( scope, file, discriminator );
        }

        /// <inheritdoc/>
        public DISubProgram CreateFunction( DIScope? scope, LazyEncodedString name, LazyEncodedString mangledName, DIFile? file, uint line, DISubroutineType? signatureType, bool isLocalToUnit, bool isDefinition, uint scopeLine, DebugInfoFlags debugFlags, bool isOptimized, Function function )
        {
            return Impl.CreateFunction( scope, name, mangledName, file, line, signatureType, isLocalToUnit, isDefinition, scopeLine, debugFlags, isOptimized, function );
        }

        /// <inheritdoc/>
        public DISubProgram ForwardDeclareFunction( DIScope? scope, LazyEncodedString name, LazyEncodedString mangledName, DIFile? file, uint line, DISubroutineType subroutineType, bool isLocalToUnit, bool isDefinition, uint scopeLine, DebugInfoFlags debugFlags, bool isOptimized )
        {
            return Impl.ForwardDeclareFunction( scope, name, mangledName, file, line, subroutineType, isLocalToUnit, isDefinition, scopeLine, debugFlags, isOptimized );
        }

        /// <inheritdoc/>
        public DILocalVariable CreateLocalVariable( DIScope? scope, LazyEncodedString name, DIFile? file, uint line, DIType? type, bool alwaysPreserve = false, DebugInfoFlags debugFlags = DebugInfoFlags.None, uint alignInBits = 0 )
        {
            return Impl.CreateLocalVariable( scope, name, file, line, type, alwaysPreserve, debugFlags, alignInBits );
        }

        /// <inheritdoc/>
        public DILocalVariable CreateArgument( DIScope? scope, LazyEncodedString name, DIFile? file, uint line, DIType? type, bool alwaysPreserve, DebugInfoFlags debugFlags, ushort argNo )
        {
            return Impl.CreateArgument( scope, name, file, line, type, alwaysPreserve, debugFlags, argNo );
        }

        /// <inheritdoc/>
        public DIBasicType CreateBasicType( LazyEncodedString name, ulong bitSize, DiTypeKind encoding, DebugInfoFlags diFlags = DebugInfoFlags.None )
        {
            return Impl.CreateBasicType( name, bitSize, encoding, diFlags );
        }

        /// <inheritdoc/>
        public DIDerivedType CreatePointerType( DIType? pointeeType, LazyEncodedString? name, ulong bitSize, uint bitAlign = 0, uint addressSpace = 0 )
        {
            return Impl.CreatePointerType( pointeeType, name, bitSize, bitAlign, addressSpace );
        }

        /// <inheritdoc/>
        public DIDerivedType CreateQualifiedType( DIType? baseType, QualifiedTypeTag tag )
        {
            return Impl.CreateQualifiedType( baseType, tag );
        }

        /// <inheritdoc/>
        public DITypeArray CreateTypeArray( IEnumerable<DIType?> types )
        {
            return Impl.CreateTypeArray( types );
        }

        /// <inheritdoc/>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, params DIType?[] types )
        {
            return Impl.CreateSubroutineType( debugFlags, types );
        }

        /// <inheritdoc/>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, IEnumerable<DIType?> types )
        {
            return Impl.CreateSubroutineType( debugFlags, types );
        }

        /// <inheritdoc/>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags )
        {
            return Impl.CreateSubroutineType( debugFlags );
        }

        /// <inheritdoc/>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DIType? returnType, IEnumerable<DIType?> types )
        {
            return Impl.CreateSubroutineType( debugFlags, returnType, types );
        }

        /// <inheritdoc/>
        public DICompositeType CreateStructType( DIScope? scope, LazyEncodedString name, DIFile? file, uint line, ulong bitSize, uint bitAlign, DebugInfoFlags debugFlags, DIType? derivedFrom, params DINode[] elements )
        {
            return Impl.CreateStructType( scope, name, file, line, bitSize, bitAlign, debugFlags, derivedFrom, elements );
        }

        /// <inheritdoc/>
        public DICompositeType CreateStructType( DIScope? scope, LazyEncodedString name, DIFile? file, uint line, ulong bitSize, uint bitAlign, DebugInfoFlags debugFlags, DIType? derivedFrom, IEnumerable<DINode> elements, uint runTimeLang = 0, DIType? vTableHolder = null, LazyEncodedString? uniqueId = null )
        {
            return Impl.CreateStructType( scope, name, file, line, bitSize, bitAlign, debugFlags, derivedFrom, elements, runTimeLang, vTableHolder, uniqueId );
        }

        /// <inheritdoc/>
        public DICompositeType CreateUnionType( DIScope? scope, LazyEncodedString name, DIFile? file, uint line, ulong bitSize, uint bitAlign, DebugInfoFlags debugFlags, DINodeArray elements )
        {
            return Impl.CreateUnionType( scope, name, file, line, bitSize, bitAlign, debugFlags, elements );
        }

        /// <inheritdoc/>
        public DICompositeType CreateUnionType( DIScope? scope, LazyEncodedString name, DIFile? file, uint line, ulong bitSize, uint bitAlign, DebugInfoFlags debugFlags, params DINode[] elements )
        {
            return Impl.CreateUnionType( scope, name, file, line, bitSize, bitAlign, debugFlags, elements );
        }

        /// <inheritdoc/>
        public DICompositeType CreateUnionType( DIScope? scope, LazyEncodedString name, DIFile? file, uint line, ulong bitSize, uint bitAlign, DebugInfoFlags debugFlags, IEnumerable<DINode> elements, uint runTimeLang = 0, LazyEncodedString? uniqueId = null )
        {
            return Impl.CreateUnionType( scope, name, file, line, bitSize, bitAlign, debugFlags, elements, runTimeLang, uniqueId );
        }

        /// <inheritdoc/>
        public DIDerivedType CreateMemberType( DIScope? scope, LazyEncodedString name, DIFile? file, uint line, ulong bitSize, uint bitAlign, ulong bitOffset, DebugInfoFlags debugFlags, DIType? type )
        {
            return Impl.CreateMemberType( scope, name, file, line, bitSize, bitAlign, bitOffset, debugFlags, type );
        }

        /// <inheritdoc/>
        public DICompositeType CreateArrayType( ulong bitSize, uint bitAlign, DIType elementType, DINodeArray subscripts )
        {
            return Impl.CreateArrayType( bitSize, bitAlign, elementType, subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateArrayType( ulong bitSize, uint bitAlign, DIType elementType, params DINode[] subscripts )
        {
            return Impl.CreateArrayType( bitSize, bitAlign, elementType, subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateArrayType( ulong bitSize, uint bitAlign, DIType elementType, IEnumerable<DINode> subscripts )
        {
            return Impl.CreateArrayType( bitSize, bitAlign, elementType, subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateVectorType( ulong bitSize, uint bitAlign, DIType elementType, DINodeArray subscripts )
        {
            return Impl.CreateVectorType( bitSize, bitAlign, elementType, subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateVectorType( ulong bitSize, uint bitAlign, DIType elementType, params DINode[] subscripts )
        {
            return Impl.CreateVectorType( bitSize, bitAlign, elementType, subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateVectorType( ulong bitSize, uint bitAlign, DIType elementType, IEnumerable<DINode> subscripts )
        {
            return Impl.CreateVectorType( bitSize, bitAlign, elementType, subscripts );
        }

        /// <inheritdoc/>
        public DIDerivedType CreateTypedef( DIType? type, LazyEncodedString name, DIFile? file, uint line, DINode? context, uint alignInBits )
        {
            return Impl.CreateTypedef( type, name, file, line, context, alignInBits );
        }

        /// <inheritdoc/>
        public DISubRange CreateSubRange( long lowerBound, long count )
        {
            return Impl.CreateSubRange( lowerBound, count );
        }

        /// <inheritdoc/>
        public DINodeArray GetOrCreateArray( IEnumerable<DINode> elements )
        {
            return Impl.GetOrCreateArray( elements );
        }

        /// <inheritdoc/>
        public DITypeArray GetOrCreateTypeArray( params IEnumerable<DIType> types )
        {
            return Impl.GetOrCreateTypeArray( types );
        }

        /// <inheritdoc/>
        public DIEnumerator CreateEnumeratorValue( LazyEncodedString name, long value, bool isUnsigned = false )
        {
            return Impl.CreateEnumeratorValue( name, value, isUnsigned );
        }

        /// <inheritdoc/>
        public DICompositeType CreateEnumerationType( DIScope? scope, LazyEncodedString name, DIFile? file, uint lineNumber, ulong sizeInBits, uint alignInBits, IEnumerable<DIEnumerator> elements, DIType? underlyingType )
        {
            return Impl.CreateEnumerationType( scope, name, file, lineNumber, sizeInBits, alignInBits, elements, underlyingType );
        }

        /// <inheritdoc/>
        public DIGlobalVariableExpression CreateGlobalVariableExpression( DINode? scope, LazyEncodedString name, LazyEncodedString linkageName, DIFile? file, uint lineNo, DIType? type, bool isLocalToUnit, DIExpression? value, DINode? declaration = null, uint bitAlign = 0 )
        {
            return Impl.CreateGlobalVariableExpression( scope, name, linkageName, file, lineNo, type, isLocalToUnit, value, declaration, bitAlign );
        }

        /// <inheritdoc/>
        public void Finish( DISubProgram subProgram )
        {
            Impl.Finish( subProgram );
        }

        /// <inheritdoc/>
        public void Finish( )
        {
            Impl.Finish();
        }

        /// <inheritdoc/>
        public DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, Instruction insertBefore )
        {
            return Impl.InsertDeclare( storage, varInfo, location, insertBefore );
        }

        /// <inheritdoc/>
        public DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DIExpression expression, DILocation location, Instruction insertBefore )
        {
            return Impl.InsertDeclare( storage, varInfo, expression, location, insertBefore );
        }

        /// <inheritdoc/>
        public DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, BasicBlock insertAtEnd )
        {
            return Impl.InsertDeclare( storage, varInfo, location, insertAtEnd );
        }

        /// <inheritdoc/>
        public DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DIExpression expression, DILocation location, BasicBlock insertAtEnd )
        {
            return Impl.InsertDeclare( storage, varInfo, expression, location, insertAtEnd );
        }

        /// <inheritdoc/>
        public DebugRecord InsertValue( Value value, DILocalVariable varInfo, DILocation location, Instruction insertBefore )
        {
            return Impl.InsertValue( value, varInfo, location, insertBefore );
        }

        /// <inheritdoc/>
        public DebugRecord InsertValue( Value value, DILocalVariable varInfo, DIExpression? expression, DILocation location, Instruction insertBefore )
        {
            return Impl.InsertValue( value, varInfo, expression, location, insertBefore );
        }

        /// <inheritdoc/>
        public DebugRecord InsertValue( Value value, DILocalVariable varInfo, DILocation location, BasicBlock insertAtEnd )
        {
            return Impl.InsertValue( value, varInfo, location, insertAtEnd );
        }

        /// <inheritdoc/>
        public DebugRecord InsertValue( Value value, DILocalVariable varInfo, DIExpression? expression, DILocation location, BasicBlock insertAtEnd )
        {
            return Impl.InsertValue( value, varInfo, expression, location, insertAtEnd );
        }

        /// <inheritdoc/>
        public DIExpression CreateExpression( params IEnumerable<ExpressionOp> operations )
        {
            return Impl.CreateExpression( operations );
        }

        /// <inheritdoc/>
        public DIExpression CreateConstantValueExpression( ulong value )
        {
            return Impl.CreateConstantValueExpression( value );
        }

        /// <inheritdoc/>
        public DICompositeType CreateReplaceableCompositeType( Tag tag, LazyEncodedString name, DIScope? scope, DIFile? file, uint line, uint lang = 0, ulong sizeInBits = 0, uint alignBits = 0, DebugInfoFlags flags = DebugInfoFlags.None, LazyEncodedString? uniqueId = null )
        {
            return Impl.CreateReplaceableCompositeType( tag, name, scope, file, line, lang, sizeInBits, alignBits, flags, uniqueId );
        }
        #endregion

        /// <summary>Gets a value indicating whether this instance is already disposed</summary>
        public bool IsDisposed => NativeHandle is null || NativeHandle.IsInvalid || NativeHandle.IsClosed;

        // keeping this private for now as there doesn't seem to be a good reason to support
        // allowUnresolved == false
        private DIBuilder( IModule owningModule, bool allowUnresolved )
        {
            ArgumentNullException.ThrowIfNull( owningModule );
            var unownedModuleHandle = owningModule.GetUnownedHandle();
            NativeHandle = allowUnresolved
                ? LLVMCreateDIBuilder( unownedModuleHandle )
                : LLVMCreateDIBuilderDisallowUnresolved( unownedModuleHandle );

            AliasImpl = new(NativeHandle, owningModule);
        }

        #region IGlobalHandleOwner<LLVMDIBuilderRef> (Pattern)

        /// <inheritdoc/>
        [SuppressMessage( "StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "internal interface" )]
        LLVMDIBuilderRef IGlobalHandleOwner<LLVMDIBuilderRef>.OwnedHandle => NativeHandle;

        /// <inheritdoc/>
        void IGlobalHandleOwner<LLVMDIBuilderRef>.InvalidateFromMove( ) => NativeHandle.SetHandleAsInvalid();

        private DIBuilderAlias Impl
        {
            get
            {
                ObjectDisposedException.ThrowIf( IsDisposed, this );
                return AliasImpl;
            }
        }

        private readonly DIBuilderAlias AliasImpl;
        private readonly LLVMDIBuilderRef NativeHandle;
        #endregion
    }
}
