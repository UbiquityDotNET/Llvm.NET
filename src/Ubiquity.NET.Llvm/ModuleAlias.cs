// -----------------------------------------------------------------------
// <copyright file="ModuleAlias.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    internal sealed class ModuleAlias
        : IModule
        , IHandleWrapper<LLVMModuleRefAlias>
        , IEquatable<IModule>
        , IEquatable<ModuleAlias>
    {
        #region IEquatable<>

        /// <inheritdoc/>
        public bool Equals(IModule? other) => other is not null && NativeHandle.Equals(other.GetUnownedHandle());

        /// <inheritdoc/>
        public bool Equals(ModuleAlias? other) => other is not null && NativeHandle.Equals(other.NativeHandle);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is ModuleAlias alias
                                                  ? Equals( alias )
                                                  : Equals( obj as IModule);

        /// <inheritdoc/>
        public override int GetHashCode() => NativeHandle.GetHashCode();
        #endregion

        /// <inheritdoc/>
        public string SourceFileName
        {
            get => LibLLVMGetModuleSourceFileName( NativeHandle ) ?? string.Empty;

            set => LibLLVMSetModuleSourceFileName( NativeHandle, value );
        }

        /// <inheritdoc/>
        public ComdatCollection Comdats => new(this);

        /// <inheritdoc/>
        public IContext Context => new ContextAlias( LLVMGetModuleContext( NativeHandle ) );

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, ModuleFlag> ModuleFlags
        {
            get
            {
                var retVal = new Dictionary<string, ModuleFlag>( );
                using(LLVMModuleFlagEntry flags = LLVMCopyModuleFlagsMetadata( NativeHandle, out size_t len ))
                {
                    for(uint i = 0; i < len; ++i)
                    {
                        var behavior = LLVMModuleFlagEntriesGetFlagBehavior( flags, i );
                        string? key = LLVMModuleFlagEntriesGetKey( flags, i, out size_t _ );
                        Debug.Assert( key is not null, "Internal LLVM error; should never have a null key" );
                        var metadata = LLVMModuleFlagEntriesGetMetadata( flags, i ).CreateMetadata();
                        Debug.Assert( metadata is not null, "Internal LLVM error; should never have null metadata");
                        retVal.Add( key, new ModuleFlag( (ModuleFlagBehavior)behavior, key, metadata ) );
                    }
                }

                return retVal;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<DICompileUnit> CompileUnits
            => from node in NamedMetadata
               where node.Name == "llvm.dbg.cu"
               from operand in node.Operands
               let cu = (DICompileUnit)operand
               where cu.EmissionKind != DwarfEmissionKind.None
               select cu;

        /// <inheritdoc/>
        [DisallowNull]
        public LazyEncodedString DataLayoutString
        {
            get
            {
                var dataLayout = LLVMGetModuleDataLayout( NativeHandle );
                return dataLayout.ToString() ?? string.Empty;
            }

            set
            {
                unsafe
                {
                    ArgumentNullException.ThrowIfNull( value );

                    using var memHandle = value.Pin();
                    LLVMSetDataLayout( NativeHandle, (byte*)memHandle.Pointer );
                }
            }
        }

        /// <inheritdoc/>
        public IDataLayout Layout
        {
            get => new DataLayoutAlias( LLVMGetModuleDataLayout( NativeHandle ) );

            set
            {
                ArgumentNullException.ThrowIfNull( value );
                DataLayoutString = value.ToLazyEncodedString();
            }
        }

        /// <inheritdoc/>
        public string TargetTriple
        {
            get => LLVMGetTarget( NativeHandle ) ?? string.Empty;

            set => LLVMSetTarget( NativeHandle, value );
        }

        /// <inheritdoc/>
        public IEnumerable<GlobalVariable> Globals
        {
            get
            {
                var current = LLVMGetFirstGlobal( NativeHandle );
                while(current != default)
                {
                    yield return Value.FromHandle<GlobalVariable>( current )!;
                    current = LLVMGetNextGlobal( current );
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Function> Functions
        {
            get
            {
                var current = LLVMGetFirstFunction( NativeHandle );
                while(current != default)
                {
                    yield return Value.FromHandle<Function>( current )!;
                    current = LLVMGetNextFunction( current );
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<GlobalAlias> Aliases
        {
            get
            {
                var current = LibLLVMModuleGetFirstGlobalAlias( NativeHandle );
                while(current != default)
                {
                    yield return Value.FromHandle<GlobalAlias>( current )!;
                    current = LibLLVMModuleGetNextGlobalAlias( current );
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<NamedMDNode> NamedMetadata
        {
            get
            {
                var current = LLVMGetFirstNamedMetadata( NativeHandle );
                while(current != default)
                {
                    yield return new NamedMDNode( current );
                    current = LLVMGetNextNamedMetadata( current );
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<GlobalIFunc> IndirectFunctions
        {
            get
            {
                var current = LLVMGetFirstGlobalIFunc( NativeHandle );
                while(current != default)
                {
                    yield return Value.FromHandle<GlobalIFunc>( current )!;
                    current = LLVMGetNextGlobalIFunc( current );
                }
            }
        }

        /// <inheritdoc/>
        public string Name => LibLLVMGetModuleName( NativeHandle ) ?? string.Empty;

        /// <inheritdoc/>
        public string ModuleInlineAsm
        {
            get => LLVMGetModuleInlineAsm( NativeHandle, out size_t _ ) ?? string.Empty;

            set => LLVMSetModuleInlineAsm2( NativeHandle, value, string.IsNullOrEmpty( value ) ? 0 : value.Length );
        }

        /// <inheritdoc/>
        public void AppendInlineAsm(string asm)
        {
            LLVMAppendModuleInlineAsm( NativeHandle, asm, string.IsNullOrEmpty( asm ) ? 0 : asm.Length );
        }

        /// <inheritdoc/>
        public ErrorInfo TryRunPasses(params string[] passes)
        {
            using PassBuilderOptions options = new();
            return TryRunPasses( options, passes );
        }

        /// <inheritdoc/>
        public ErrorInfo TryRunPasses(PassBuilderOptions options, params string[] passes)
        {
            ArgumentNullException.ThrowIfNull( passes );
            ArgumentOutOfRangeException.ThrowIfLessThan( passes.Length, 1 );

            // TODO: [Optimization] provide overload that accepts a ReadOnlySpan<byte> for the pass "string"
            //       That way the overhead of Join() and conversion to native form is performed exactly once.
            //       (NativeStringView ~= ReadOnlySpan<byte>?)

            // While not explicitly documented either way, the PassBuilder used under the hood is capable
            // of handling a NULL target machine.
            return new( LLVMRunPasses( NativeHandle, string.Join( ',', passes ), LLVMTargetMachineRef.Zero, options.Handle ) );
        }

        /// <inheritdoc/>
        public ErrorInfo TryRunPasses(TargetMachine targetMachine, PassBuilderOptions options, params string[] passes)
        {
            ArgumentNullException.ThrowIfNull( targetMachine );
            ArgumentNullException.ThrowIfNull( passes );
            ArgumentOutOfRangeException.ThrowIfLessThan( passes.Length, 1 );

            return new( LLVMRunPasses( NativeHandle, string.Join( ',', passes ), targetMachine.Handle, options.Handle ) );
        }

        /// <inheritdoc/>
        public void Link(Module srcModule)
        {
            ArgumentNullException.ThrowIfNull( srcModule );

            if(!Context.Equals(srcModule.Context))
            {
                throw new ArgumentException( Resources.Linking_modules_from_different_contexts_is_not_allowed, nameof( srcModule ) );
            }

            if(!LLVMLinkModules2( NativeHandle, srcModule.GetOwnedHandle() ).Succeeded)
            {
                throw new InternalCodeGeneratorException( Resources.Module_link_error );
            }

            // The native source module was already destroyed mark it as no longer usable.
            srcModule.InvalidateFromMove();
        }

        /// <inheritdoc/>
        public bool Verify(out string errorMessage)
        {
            return LLVMVerifyModule( NativeHandle, LLVMVerifierFailureAction.LLVMReturnStatusAction, out errorMessage ).Succeeded;
        }

        /// <inheritdoc/>
        public bool TryGetFunction(string name, [MaybeNullWhen( false )] out Function function)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var funcRef = LLVMGetNamedFunction( NativeHandle, name );
            if(funcRef == default)
            {
                function = null;
                return false;
            }

            function = Value.FromHandle<Function>( funcRef )!;
            return true;
        }

        /// <inheritdoc/>
        public GlobalIFunc CreateAndAddGlobalIFunc(string name, ITypeRef type, uint addressSpace, Function resolver)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            ArgumentNullException.ThrowIfNull( type );
            ArgumentNullException.ThrowIfNull( resolver );

            var handle = LLVMAddGlobalIFunc( NativeHandle, name, name.Length, type.GetTypeRef( ), addressSpace, resolver.Handle );
            return Value.FromHandle<GlobalIFunc>( handle.ThrowIfInvalid() )!;
        }

        /// <inheritdoc/>
        public bool TryGetNamedGlobalIFunc(string name, [MaybeNullWhen( false )] out GlobalIFunc function)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var funcRef = LLVMGetNamedGlobalIFunc( NativeHandle, name, name.Length );
            if(funcRef == default)
            {
                function = null;
                return false;
            }

            function = Value.FromHandle<GlobalIFunc>( funcRef )!;
            return true;
        }

        /// <inheritdoc/>
        public Function CreateFunction(string name, IFunctionType signature)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            ArgumentNullException.ThrowIfNull( signature );

            LLVMValueRef function = LLVMGetNamedFunctionWithLength(NativeHandle, name, name.Length);
            if(function.IsNull)
            {
                function = LLVMAddFunction( NativeHandle, name, signature.GetTypeRef() );
            }

            return Value.FromHandle<Function>( function.ThrowIfInvalid() )!;
        }

        /// <inheritdoc/>
        public void WriteToFile(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( path );

            LLVMStatus status = LLVMWriteBitcodeToFile( NativeHandle, path );
            if(status.Failed)
            {
                throw new IOException( string.Format( CultureInfo.CurrentCulture, Resources.Error_writing_bit_code_file_0, path ), status.ErrorCode );
            }
        }

        /// <inheritdoc/>
        public bool WriteToTextFile(string path, out string errMsg)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( path );

            return LLVMPrintModuleToFile( NativeHandle, path, out errMsg ).Succeeded;
        }

        /// <inheritdoc/>
        public string? WriteToString()
        {
            return LLVMPrintModuleToString( NativeHandle );
        }

        /// <inheritdoc/>
        public MemoryBuffer WriteToBuffer()
        {
            return new MemoryBuffer( LLVMWriteBitcodeToMemoryBuffer( NativeHandle ) );
        }

        /// <inheritdoc/>
        public GlobalAlias AddAlias(Value aliasee, string aliasName, uint addressSpace = 0)
        {
            ArgumentNullException.ThrowIfNull( aliasee );
            ArgumentException.ThrowIfNullOrWhiteSpace( aliasName );

            var handle = LLVMAddAlias2( NativeHandle, aliasee.NativeType.GetTypeRef( ), addressSpace, aliasee.Handle, aliasName );
            return Value.FromHandle<GlobalAlias>( handle.ThrowIfInvalid() )!;
        }

        /// <inheritdoc/>
        public GlobalAlias? GetAlias(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LibLLVMGetGlobalAlias( NativeHandle, name );
            return Value.FromHandle<GlobalAlias>( handle );
        }

        /// <inheritdoc/>
        public GlobalVariable AddGlobalInAddressSpace(uint addressSpace, ITypeRef typeRef, string name)
        {
            ArgumentNullException.ThrowIfNull( typeRef );
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMAddGlobalInAddressSpace( NativeHandle, typeRef.GetTypeRef( ), name, addressSpace );
            return Value.FromHandle<GlobalVariable>( handle.ThrowIfInvalid() )!;
        }

        /// <inheritdoc/>
        public GlobalVariable AddGlobalInAddressSpace(uint addressSpace, ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal)
        {
            ArgumentNullException.ThrowIfNull( typeRef );
            linkage.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( constVal );

            return AddGlobalInAddressSpace( addressSpace, typeRef, isConst, linkage, constVal, string.Empty );
        }

        /// <inheritdoc/>
        public GlobalVariable AddGlobalInAddressSpace(uint addressSpace, ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal, string name)
        {
            ArgumentNullException.ThrowIfNull( typeRef );
            linkage.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( constVal );
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var retVal = AddGlobalInAddressSpace( addressSpace, typeRef, name );
            retVal.IsConstant = isConst;
            retVal.Linkage = linkage;
            retVal.Initializer = constVal;
            return retVal;
        }

        /// <inheritdoc/>
        public GlobalVariable AddGlobal(ITypeRef typeRef, string name)
        {
            ArgumentNullException.ThrowIfNull( typeRef );
            ArgumentNullException.ThrowIfNull( name );

            var handle = LLVMAddGlobal( NativeHandle, typeRef.GetTypeRef( ), name );
            return Value.FromHandle<GlobalVariable>( handle.ThrowIfInvalid() )!;
        }

        /// <inheritdoc/>
        public GlobalVariable AddGlobal(ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal)
        {
            ArgumentNullException.ThrowIfNull( typeRef );
            linkage.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( constVal );

            return AddGlobal( typeRef, isConst, linkage, constVal, string.Empty );
        }

        /// <inheritdoc/>
        public GlobalVariable AddGlobal(ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal, string name)
        {
            ArgumentNullException.ThrowIfNull( typeRef );
            linkage.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( constVal );
            ArgumentNullException.ThrowIfNull( name );

            var retVal = AddGlobal( typeRef, name );
            retVal.IsConstant = isConst;
            retVal.Linkage = linkage;
            retVal.Initializer = constVal;
            return retVal;
        }

        /// <inheritdoc/>
        public ITypeRef? GetTypeByName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            LLVMTypeRef hType = LLVMGetTypeByName( NativeHandle, name );
            return hType == default ? null : hType.CreateType();
        }

        /// <inheritdoc/>
        public GlobalVariable? GetNamedGlobal(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var hGlobal = LLVMGetNamedGlobal( NativeHandle, name );
            return hGlobal == default ? null : Value.FromHandle<GlobalVariable>( hGlobal );
        }

        /// <inheritdoc/>
        public void AddModuleFlag(ModuleFlagBehavior behavior, string name, UInt32 value)
        {
            behavior.ThrowIfNotDefined();
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var metadata = Context.CreateConstant( value ).ToMetadata();

            LLVMAddModuleFlag( NativeHandle, (LLVMModuleFlagBehavior)behavior, name, name.Length, metadata.Handle );
        }

        /// <inheritdoc/>
        public void AddModuleFlag(ModuleFlagBehavior behavior, string name, IrMetadata value)
        {
            behavior.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( value );
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            LLVMAddModuleFlag( NativeHandle, (LLVMModuleFlagBehavior)behavior, name, name.Length, value.Handle );
        }

        /// <inheritdoc/>
        public void AddNamedMetadataOperand(string name, IrMetadata value)
        {
            ArgumentNullException.ThrowIfNull( value );
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            LibLLVMAddNamedMetadataOperand2( NativeHandle, name, value?.Handle ?? default );
        }

        /// <inheritdoc/>
        public void AddVersionIdentMetadata(string version)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( version );

            var stringNode = Context.CreateMDNode( version );
            LibLLVMAddNamedMetadataOperand2( NativeHandle, "llvm.ident", stringNode.Handle );
        }

        /// <inheritdoc/>
        public Function CreateFunction( ref readonly DIBuilder diBuilder
                                      , DIScope? scope
                                      , string name
                                      , string? linkageName
                                      , DIFile? file
                                      , uint line
                                      , DebugFunctionType signature
                                      , bool isLocalToUnit
                                      , bool isDefinition
                                      , uint scopeLine
                                      , DebugInfoFlags debugFlags
                                      , bool isOptimized
                                      )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            ArgumentNullException.ThrowIfNull( signature );

            if (!Equals(diBuilder.OwningModule))
            {
                throw new ArgumentException("Builder created for a different module!", nameof(diBuilder));
            }

            if(string.IsNullOrWhiteSpace( linkageName ))
            {
                linkageName = name;
            }

            if(signature.DebugInfoType == null)
            {
                throw new ArgumentException( Resources.Signature_requires_debug_type_information, nameof( signature ) );
            }

            var func = CreateFunction( linkageName, signature );
            var diSignature = signature.DebugInfoType;
            var diFunc = diBuilder.CreateFunction( scope: scope
                                                 , name: name
                                                 , mangledName: linkageName
                                                 , file: file
                                                 , line: line
                                                 , signatureType: diSignature
                                                 , isLocalToUnit: isLocalToUnit
                                                 , isDefinition: isDefinition
                                                 , scopeLine: scopeLine
                                                 , debugFlags: debugFlags
                                                 , isOptimized: isOptimized
                                                 , function: func
                                                 );

            Debug.Assert( diFunc.Describes( func ), "Expected to get a debug function that describes the provided function" );
            func.DISubProgram = diFunc;
            return func;
        }

        /// <inheritdoc/>
        public Function CreateFunction( ref readonly DIBuilder diBuilder
                                      , string name
                                      , bool isVarArg
                                      , IDebugType<ITypeRef, DIType> returnType
                                      , IEnumerable<IDebugType<ITypeRef, DIType>> argumentTypes
                                      )
        {
            IFunctionType signature = Context.CreateFunctionType( in diBuilder, isVarArg, returnType, argumentTypes );
            return CreateFunction( name, signature );
        }

        /// <inheritdoc/>
        public Function CreateFunction( ref readonly DIBuilder diBuilder
                                      , string name
                                      , bool isVarArg
                                      , IDebugType<ITypeRef, DIType> returnType
                                      , params IDebugType<ITypeRef, DIType>[] argumentTypes
                                      )
        {
            return CreateFunction( in diBuilder, name, isVarArg, returnType, (IEnumerable<IDebugType<ITypeRef, DIType>>)argumentTypes );
        }

        /// <inheritdoc/>
        public Function GetIntrinsicDeclaration(string name, params ITypeRef[] args)
        {
            uint id = Intrinsic.LookupId( name );
            return GetIntrinsicDeclaration( id, args );
        }

        /// <inheritdoc/>
        public Function GetIntrinsicDeclaration(UInt32 id, params ITypeRef[] args)
        {
            ArgumentNullException.ThrowIfNull( args );

            if(!LLVMIntrinsicIsOverloaded( id ) && args.Length > 0)
            {
                throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Intrinsic_0_is_not_overloaded_and_therefore_does_not_require_type_arguments, id ) );
            }

            LLVMTypeRef[ ] llvmArgs = [ .. args.Select( a => a.GetTypeRef( ) ) ];
            LLVMValueRef valueRef = LLVMGetIntrinsicDeclaration( NativeHandle, id, llvmArgs, llvmArgs.Length );
            return (Function)Value.FromHandle( valueRef.ThrowIfInvalid() )!;
        }

        /// <inheritdoc/>
        public Module Clone()
        {
            return new( LLVMCloneModule( NativeHandle ).ThrowIfInvalid() );
        }

        /// <inheritdoc/>
        public Module Clone(IContext targetContext)
        {
            ArgumentNullException.ThrowIfNull( targetContext );

            // optimization if it is the same context, just use existing native support
            if(targetContext.Equals(Context))
            {
                return Clone();
            }

            // LLVM has no direct API for cloning into another context
            // so write it to a buffer, then reload that buffer into another context.
            using var buffer = WriteToBuffer( );
            var retVal = Module.LoadFrom( buffer, targetContext );
            Debug.Assert( retVal.Context.Equals(targetContext), Resources.Expected_to_get_a_module_bound_to_the_specified_context );
            return retVal;
        }

        internal ModuleAlias(LLVMModuleRefAlias handle)
        {
            handle.ThrowIfInvalid();
            NativeHandle = handle;
        }

        [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Interface is not public" )]
        [SuppressMessage( "StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Again, NOT public!" )]
        LLVMModuleRefAlias IHandleWrapper<LLVMModuleRefAlias>.Handle => NativeHandle;

        // must use a distinct field so that it is useful as an "in" parameter
        // to implementations of IEnumerable<T> where T is a ref struct
        private readonly LLVMModuleRefAlias NativeHandle;
    }
}
