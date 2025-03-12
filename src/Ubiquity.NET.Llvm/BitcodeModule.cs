// -----------------------------------------------------------------------
// <copyright file="BitcodeModule.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    /// <summary>Enumeration to indicate the behavior of module level flags metadata sharing the same name in a <see cref="BitcodeModule"/></summary>
    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "It isn't a flags enum" )]
    public enum ModuleFlagBehavior
    {
        /// <summary>Emits an error if two values disagree, otherwise the resulting value is that of the operands</summary>
        Error = LLVMModuleFlagBehavior.LLVMModuleFlagBehaviorError,

        /// <summary>Emits a warning if two values disagree. The result will be the operand for the flag from the first module being linked</summary>
        Warning = LLVMModuleFlagBehavior.LLVMModuleFlagBehaviorWarning,

        /// <summary>Adds a requirement that another module flag be present and have a specified value after linking is performed</summary>
        /// <remarks>
        /// The value must be a metadata pair, where the first element of the pair is the ID of the module flag to be restricted, and the
        /// second element of the pair is the value the module flag should be restricted to. This behavior can be used to restrict the
        /// allowable results (via triggering of an error) of linking IDs with the <see cref="Override"/> behavior
        /// </remarks>
        Require = LLVMModuleFlagBehavior.LLVMModuleFlagBehaviorRequire,

        /// <summary>Uses the specified value, regardless of the behavior or value of the other module</summary>
        /// <remarks>If both modules specify Override, but the values differ, and error will be emitted</remarks>
        Override = LLVMModuleFlagBehavior.LLVMModuleFlagBehaviorOverride,

        /// <summary>Appends the two values, which are required to be metadata nodes</summary>
        Append = LLVMModuleFlagBehavior.LLVMModuleFlagBehaviorAppend,

        /// <summary>Appends the two values, which are required to be metadata nodes dropping duplicate entries in the second list</summary>
        AppendUnique = LLVMModuleFlagBehavior.LLVMModuleFlagBehaviorAppendUnique,
    }

    /// <summary>LLVM Bit-code module</summary>
    /// <remarks>
    /// A module is the basic unit for containing code in LLVM. Modules are an in memory
    /// representation of the LLVM Intermediate Representation (IR) bit-code. Each
    /// </remarks>
    public sealed class BitcodeModule
        : IDisposable
    {
        /// <summary>Gets a value indicating whether the module is disposed or not</summary>
        public bool IsDisposed => ( ModuleHandle is null ) || ModuleHandle.IsInvalid || ModuleHandle.IsClosed;

        /// <summary>Gets or sets the name of the source file generating this module</summary>
        public string SourceFileName
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                return LibLLVMGetModuleSourceFileName( ModuleHandle! ) ?? string.Empty;
            }

            set
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                LibLLVMSetModuleSourceFileName( ModuleHandle!, value );
            }
        }

        /// <summary>Name of the Debug Version information module flag</summary>
        public const string DebugVersionValue = "Debug Info Version";

        /// <summary>Name of the Dwarf Version module flag</summary>
        public const string DwarfVersionValue = "Dwarf Version";

        /// <summary>Version of the Debug information LlvmMetadata</summary>
        public const UInt32 DebugMetadataVersion = 3; /* DEBUG_METADATA_VERSION (for LLVM > v3.7.0) */

        /// <summary>Gets the Comdats for this module</summary>
        public ComdatCollection Comdats { get; }

        /// <summary>Gets the <see cref="Context"/> this module belongs to</summary>
        public Context Context { get; }

        /// <summary>Gets the LlvmMetadata for module level flags</summary>
        public IReadOnlyDictionary<string, ModuleFlag> ModuleFlags
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                var retVal = new Dictionary<string, ModuleFlag>( );
                using( LLVMModuleFlagEntry flags = LLVMCopyModuleFlagsMetadata( ModuleHandle!, out size_t len ) )
                {
                    for( uint i = 0; i < len; ++i )
                    {
                        var behavior = LLVMModuleFlagEntriesGetFlagBehavior( flags, i );
                        string? key = LLVMModuleFlagEntriesGetKey( flags, i, out size_t _ );
                        Debug.Assert(key is not null, "Internal LLVM error; should never have a null key");
                        var metadata = LlvmMetadata.FromHandle<LlvmMetadata>( Context, LLVMModuleFlagEntriesGetMetadata( flags, i ).ThrowIfInvalid( ) );
                        retVal.Add( key, new ModuleFlag( ( ModuleFlagBehavior )behavior, key, metadata! ) );
                    }
                }

                return retVal;
            }
        }

        /// <summary>Gets the <see cref="DebugInfoBuilder"/> used to create debug information for this module</summary>
        /// <remarks>The builder returned from this property is lazy constructed on first access so doesn't consume resources unless used.</remarks>
        public DebugInfoBuilder DIBuilder
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                return LazyDiBuilder.Value;
            }
        }

        /// <summary>Gets the Debug Compile unit for this module</summary>
        public DICompileUnit? DICompileUnit { get; internal set; }

        /// <summary>Gets or Sets the Data layout string for this module</summary>
        /// <remarks>
        /// <note type="note">The data layout string doesn't do what seems obvious.
        /// That is, it doesn't force the target back-end to generate code
        /// or types with a particular layout. Rather, the layout string has
        /// to match the implicit layout of the target. Thus it should only
        /// come from the actual <see cref="TargetMachine"/> the code is
        /// targeting.</note>
        /// </remarks>
        [DisallowNull]
        public LazyEncodedString DataLayoutString
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);

                return Layout.ToString( ) ?? string.Empty;
            }

            set
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);

                unsafe
                {
                    ArgumentNullException.ThrowIfNull(value);

                    using var memHandle = value.Pin();
                    LLVMSetDataLayout(ModuleHandle!, (byte*)memHandle.Pointer);
                }
            }
        }

        /// <summary>Gets or sets the target data layout for this module</summary>
        public DataLayout Layout
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                return DataLayout.FromHandle( LLVMGetModuleDataLayout( ModuleHandle! ) );
            }

            set
            {
                ArgumentNullException.ThrowIfNull(value);
                ObjectDisposedException.ThrowIf(IsDisposed, this);

                DataLayoutString = value.ToLazyEncodedString();
            }
        }

        /// <summary>Gets or sets the Target Triple describing the target, ABI and OS</summary>
        public string TargetTriple
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                return LLVMGetTarget( ModuleHandle! ) ?? string.Empty;
            }

            set
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                LLVMSetTarget( ModuleHandle!, value );
            }
        }

        /// <summary>Gets the <see cref="GlobalVariable"/>s contained by this module</summary>
        public IEnumerable<GlobalVariable> Globals
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                var current = LLVMGetFirstGlobal( ModuleHandle! );
                while( current != default )
                {
                    yield return Value.FromHandle<GlobalVariable>( current )!;
                    current = LLVMGetNextGlobal( current );
                }
            }
        }

        /// <summary>Gets the functions contained in this module</summary>
        public IEnumerable<Function> Functions
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                var current = LLVMGetFirstFunction( ModuleHandle! );
                while( current != default )
                {
                    yield return Value.FromHandle<Function>( current )!;
                    current = LLVMGetNextFunction( current );
                }
            }
        }

        /// <summary>Gets the global aliases in this module</summary>
        public IEnumerable<GlobalAlias> Aliases
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                var current = LibLLVMModuleGetFirstGlobalAlias( ModuleHandle! );
                while( current != default )
                {
                    yield return Value.FromHandle<GlobalAlias>( current )!;
                    current = LibLLVMModuleGetNextGlobalAlias( current );
                }
            }
        }

        /// <summary>Gets the <see cref="NamedMDNode"/>s for this module</summary>
        public IEnumerable<NamedMDNode> NamedMetadata
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                var current = LLVMGetFirstNamedMetadata( ModuleHandle! );
                while( current != default )
                {
                    yield return new NamedMDNode( current );
                    current = LLVMGetNextNamedMetadata( current );
                }
            }
        }

        /// <summary>Gets the <see cref="GlobalIFunc"/>s in this module</summary>
        public IEnumerable<GlobalIFunc> IndirectFunctions
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                var current = LLVMGetFirstGlobalIFunc( ModuleHandle! );
                while( current != default )
                {
                    yield return Value.FromHandle<GlobalIFunc>( current )!;
                    current = LLVMGetNextGlobalIFunc( current );
                }
            }
        }

        /// <summary>Gets the name of the module</summary>
        public string Name => IsDisposed ? string.Empty : LibLLVMGetModuleName( ModuleHandle! ) ?? string.Empty;

        /// <summary>Gets or sets the module level inline assembly</summary>
        public string ModuleInlineAsm
        {
            get => IsDisposed ? string.Empty : LLVMGetModuleInlineAsm( ModuleHandle!, out size_t _ ) ?? string.Empty;

            set
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                LLVMSetModuleInlineAsm2( ModuleHandle!, value, string.IsNullOrEmpty( value ) ? 0 : value.Length );
            }
        }

        /// <summary>Appends inline assembly to the module's inline assembly</summary>
        /// <param name="asm">assembly text</param>
        public void AppendInlineAsm( string asm )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            LLVMAppendModuleInlineAsm( ModuleHandle!, asm, string.IsNullOrEmpty( asm ) ? 0 : asm.Length );
        }

        /// <summary>Disposes the <see cref="BitcodeModule"/>, releasing resources associated with the module in native code</summary>
        public void Dispose( )
        {
            // if not already disposed, dispose the module. Do this only on dispose. The containing context
            // will clean up the module when it is disposed or finalized. Since finalization order isn't
            // deterministic it is possible that the module is finalized after the context has already run its
            // finalizer, which would cause an access violation in the native LLVM layer.
            if( !IsDisposed )
            {
                // DI builder is owned by this module, so when it is destroyed so is the builder
                if(LazyDiBuilder.IsValueCreated)
                {
                    LazyDiBuilder.Value.BuilderHandle.SetHandleAsInvalid();
                }

                // remove the module handle from the module cache.
                ModuleHandle.Dispose( );
            }
        }

        /// <summary>Tries running the specified passes on this function</summary>
        /// <param name="passes">Set of passes to run [Must contain at least one pass]</param>
        /// <returns>Error containing result</returns>
        /// <exception cref="ArgumentNullException">One of the arguments provided was null (see exception details for name of the parameter)</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="passes"/> has less than one pass. At least one is required</exception>
        /// <remarks>
        /// This will try to run all the passes specified against this module. The return value contains
        /// the results and, if an error occurred, any error message text for the error.
        /// <note type="information">
        /// The `try` semantics apply to the actual LLVM call only, normal parameter checks are performed
        /// and may produce an exception.
        /// </note>
        /// </remarks>
        public ErrorInfo TryRunPasses(params string[] passes)
        {
            using PassBuilderOptions options = new();
            return TryRunPasses(options, passes);
        }

        /// <inheritdoc cref="TryRunPasses(string[])"/>
        /// <param name="options">Options for the passes</param>
        /// <param name="passes">Set of passes to run [Must contain at least one pass]</param>
        public ErrorInfo TryRunPasses(PassBuilderOptions options, params string[] passes)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(passes);
            ArgumentOutOfRangeException.ThrowIfLessThan(passes.Length, 1);

            // TODO: [Optimization] provide overload that accepts a ReadOnlySpan<byte> for the pass "string"
            //       That way the overhead of Join() and conversion to native form is performed exactly once.
            //       (NativeStringView ~= ReadOnlySpan<byte>?)

            // While not explicitly documented either way, the PassBuilder used under the hood is capable
            // of handling a NULL target machine.
            return new(LLVMRunPasses(ModuleHandle, string.Join(',', passes), LLVMTargetMachineRef.Zero, options.Handle));
        }

        /// <inheritdoc cref="TryRunPasses(string[])"/>
        /// <param name="targetMachine">Target machine for the passes</param>
        /// <param name="options">Options for the passes</param>
        /// <param name="passes">Set of passes to run [Must contain at least one pass]</param>
        public ErrorInfo TryRunPasses(TargetMachine targetMachine, PassBuilderOptions options, params string[] passes)
        {
            ArgumentNullException.ThrowIfNull(targetMachine);
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(passes);
            ArgumentOutOfRangeException.ThrowIfLessThan(passes.Length, 1);

            return new(LLVMRunPasses(ModuleHandle, string.Join(',', passes), targetMachine.TargetMachineHandle, options.Handle));
        }

        /// <summary>Link another module into this one</summary>
        /// <param name="otherModule">module to link into this one</param>
        /// <remarks>
        /// <note type="warning">
        /// <paramref name="otherModule"/> is destroyed by this process and no longer usable
        /// when this method returns.
        /// </note>
        /// </remarks>
        public void Link( BitcodeModule otherModule )
        {
            ArgumentNullException.ThrowIfNull(otherModule);
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ObjectDisposedException.ThrowIf(otherModule.IsDisposed, otherModule);

            if( otherModule.Context != Context )
            {
                throw new ArgumentException( Resources.Linking_modules_from_different_contexts_is_not_allowed, nameof( otherModule ) );
            }

            if( !LLVMLinkModules2( ModuleHandle!, otherModule.ModuleHandle! ).Succeeded )
            {
                throw new InternalCodeGeneratorException( Resources.Module_link_error );
            }

            Context.RemoveModule( otherModule );
            using var detachedHandle = otherModule.Detach( );
            detachedHandle.SetHandleAsInvalid( );
        }

        /// <summary>Verifies a bit-code module</summary>
        /// <param name="errorMessage">Error messages describing any issues found in the bit-code. Empty string if no error</param>
        /// <returns>true if the verification succeeded and false if not.</returns>
        public bool Verify( out string errorMessage )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            errorMessage = string.Empty;
            DisposeMessageString? llvmMsg = null;
            try
            {
                LLVMStatus retVal = LLVMVerifyModule( ModuleHandle!, LLVMVerifierFailureAction.LLVMReturnStatusAction, out llvmMsg );
                if(retVal.Failed)
                {
                    errorMessage = llvmMsg.ToString() ?? string.Empty;
                    return false;
                }

                return true;
            }
            finally
            {
                llvmMsg?.Dispose();
            }
        }

        /// <summary>Looks up a function in the module by name</summary>
        /// <param name="name">Name of the function</param>
        /// <param name="function">The function or <see langword="null"/> if not found</param>
        /// <returns><see langword="true"/> if the function was found or <see langword="false"/> if not</returns>
        public bool TryGetFunction( string name, [MaybeNullWhen( false )] out Function function )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var funcRef = LLVMGetNamedFunction( ModuleHandle!, name );
            if( funcRef == default )
            {
                function = null;
                return false;
            }

            function = Value.FromHandle<Function>( funcRef )!;
            return true;
        }

        /// <summary>Create and add a global indirect function</summary>
        /// <param name="name">Name of the function</param>
        /// <param name="type">Signature of the function</param>
        /// <param name="addressSpace">Address space for the indirect function</param>
        /// <param name="resolver">Resolver for the indirect function</param>
        /// <returns>New <see cref="GlobalIFunc"/></returns>
        public GlobalIFunc CreateAndAddGlobalIFunc( string name, ITypeRef type, uint addressSpace, Function resolver )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            ArgumentNullException.ThrowIfNull( type );
            ArgumentNullException.ThrowIfNull( resolver );

            var handle = LLVMAddGlobalIFunc( ModuleHandle!, name, name.Length, type.GetTypeRef( ), addressSpace, resolver.ValueHandle );
            return Value.FromHandle<GlobalIFunc>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Get a named Global Indirect function in the module</summary>
        /// <param name="name">Name of the ifunc to find</param>
        /// <param name="function">Function or <see langword="null"/> if not found</param>
        /// <returns><see langword="true"/> if the function was found or <see langword="false"/> if not</returns>
        public bool TryGetNamedGlobalIFunc( string name, [MaybeNullWhen( false )] out GlobalIFunc function )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var funcRef = LLVMGetNamedGlobalIFunc( ModuleHandle!, name, name.Length );
            if( funcRef == default )
            {
                function = null;
                return false;
            }

            function = Value.FromHandle<GlobalIFunc>( funcRef )!;
            return true;
        }

        /// <summary>Gets an existing function with the specified signature to the module or creates a new one if it doesn't exist</summary>
        /// <param name="name">Name of the function to add</param>
        /// <param name="signature">Signature of the function</param>
        /// <returns><see cref="Function"/>matching the specified signature and name</returns>
        /// <remarks>
        /// If a matching function already exists it is returned, and therefore the returned
        /// <see cref="Function"/> may have a body and additional attributes. If a function of
        /// the same name exists with a different signature an exception is thrown as LLVM does
        /// not perform any function overloading.
        /// </remarks>
        public Function CreateFunction( string name, IFunctionType signature )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            ArgumentNullException.ThrowIfNull( signature );

            LLVMValueRef function = LLVMGetNamedFunctionWithLength(ModuleHandle!, name, name.Length);
            if(function.IsNull)
            {
                function = LLVMAddFunction(ModuleHandle!, name, signature.GetTypeRef( ));
            }

            return Value.FromHandle<Function>( function.ThrowIfInvalid( ) )!;
        }

        /// <summary>Writes a bit-code module to a file</summary>
        /// <param name="path">Path to write the bit-code into</param>
        /// <remarks>
        /// This is a blind write. (e.g. no verification is performed)
        /// So if an invalid module is saved it might not work with any
        /// later stage processing tools.
        /// </remarks>
        public void WriteToFile( string path )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( path );

            LLVMStatus status = LLVMWriteBitcodeToFile( ModuleHandle!, path );
            if( status.Failed )
            {
                throw new IOException( string.Format( CultureInfo.CurrentCulture, Resources.Error_writing_bit_code_file_0, path ), status.ErrorCode );
            }
        }

        /// <summary>Writes this module as LLVM IR source to a file</summary>
        /// <param name="path">File to write the LLVM IR source to</param>
        /// <param name="errMsg">Error messages encountered, if any</param>
        /// <returns><see langword="true"/> if successful or <see langword="false"/> if not</returns>
        public bool WriteToTextFile( string path, out string errMsg )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( path );

            errMsg = string.Empty;
            DisposeMessageString? llvmMsg = null;
            try
            {
                LLVMStatus result = LLVMPrintModuleToFile( ModuleHandle!, path, out llvmMsg );
                if (result.Failed)
                {
                    errMsg = llvmMsg.ToString() ?? string.Empty;
                    return false;
                }

                return true;
            }
            finally
            {
                llvmMsg?.Dispose();
            }
        }

        /// <summary>Creates a string representation of the module</summary>
        /// <returns>LLVM textual representation of the module</returns>
        /// <remarks>
        /// This is intentionally NOT an override of ToString() as that is
        /// used by debuggers to show the value of a type and this can take
        /// an extremely long time (up to many seconds depending on complexity
        /// of the module) which is usually bad for the debugger. If you need
        /// to see the contents of the IR for a module you can use this method
        /// in the immediate window.
        /// </remarks>
        public string? WriteToString( )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            return LLVMPrintModuleToString( ModuleHandle! ).ToString();
        }

        /// <summary>Writes the LLVM IR bit code into a memory buffer</summary>
        /// <returns><see cref="MemoryBuffer"/> containing the bit code module</returns>
        public MemoryBuffer WriteToBuffer( )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            return new MemoryBuffer( LLVMWriteBitcodeToMemoryBuffer( ModuleHandle! ) );
        }

        /// <summary>Add an alias to the module</summary>
        /// <param name="aliasee">Value being aliased</param>
        /// <param name="aliasName">Name of the alias</param>
        /// <param name="addressSpace">Address space for the alias [Default: 0]</param>
        /// <returns><see cref="GlobalAlias"/> for the alias</returns>
        public GlobalAlias AddAlias( Value aliasee, string aliasName, uint addressSpace = 0)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentNullException.ThrowIfNull( aliasee );
            ArgumentException.ThrowIfNullOrWhiteSpace( aliasName );

            var handle = LLVMAddAlias2( ModuleHandle!, aliasee.NativeType.GetTypeRef( ), addressSpace, aliasee.ValueHandle, aliasName );
            return Value.FromHandle<GlobalAlias>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Get an alias by name</summary>
        /// <param name="name">name of the alias to get</param>
        /// <returns>Alias matching <paramref name="name"/> or null if no such alias exists</returns>
        public GlobalAlias? GetAlias( string name )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LibLLVMGetGlobalAlias( ModuleHandle!, name );
            return Value.FromHandle<GlobalAlias>( handle );
        }

        /// <summary>Adds a global to this module with a specific address space</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="name">Name of the global</param>
        /// <returns>The new <see cref="GlobalVariable"/></returns>
        /// <openissues>
        /// - What does LLVM do if creating a second Global with the same name (return null, throw, crash??,...)
        /// </openissues>
        public GlobalVariable AddGlobalInAddressSpace( uint addressSpace, ITypeRef typeRef, string name )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentNullException.ThrowIfNull( typeRef );
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMAddGlobalInAddressSpace( ModuleHandle!, typeRef.GetTypeRef( ), name, addressSpace );
            return Value.FromHandle<GlobalVariable>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobalInAddressSpace( uint addressSpace, ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentNullException.ThrowIfNull( typeRef );
            linkage.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( constVal );

            return AddGlobalInAddressSpace( addressSpace, typeRef, isConst, linkage, constVal, string.Empty );
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <param name="name">Name of the variable</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobalInAddressSpace( uint addressSpace, ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal, string name )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
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

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="name">Name of the global</param>
        /// <returns>The new <see cref="GlobalVariable"/></returns>
        /// <openissues>
        /// - What does LLVM do if creating a second Global with the same name (return null, throw, crash??,...)
        /// </openissues>
        public GlobalVariable AddGlobal( ITypeRef typeRef, string name )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentNullException.ThrowIfNull( typeRef );
            ArgumentNullException.ThrowIfNull( name );

            var handle = LLVMAddGlobal( ModuleHandle!, typeRef.GetTypeRef( ), name );
            return Value.FromHandle<GlobalVariable>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobal( ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentNullException.ThrowIfNull( typeRef );
            linkage.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( constVal );

            return AddGlobal( typeRef, isConst, linkage, constVal, string.Empty );
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <param name="name">Name of the variable</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobal( ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal, string name )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
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

        /// <summary>Retrieves a <see cref="ITypeRef"/> by name from the module</summary>
        /// <param name="name">Name of the type</param>
        /// <returns>The type or null if no type with the specified name exists in the module</returns>
        public ITypeRef? GetTypeByName( string name )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var hType = LLVMGetTypeByName( ModuleHandle!, name );
            return hType == default ? null : TypeRef.FromHandle( hType );
        }

        /// <summary>Retrieves a named global from the module</summary>
        /// <param name="name">Name of the global</param>
        /// <returns><see cref="GlobalVariable"/> or <see langword="null"/> if not found</returns>
        public GlobalVariable? GetNamedGlobal( string name )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var hGlobal = LLVMGetNamedGlobal( ModuleHandle!, name );
            return hGlobal == default ? null : Value.FromHandle<GlobalVariable>( hGlobal );
        }

        /// <summary>Adds a module flag to the module</summary>
        /// <param name="behavior">Module flag behavior for this flag</param>
        /// <param name="name">Name of the flag</param>
        /// <param name="value">Value of the flag</param>
        public void AddModuleFlag( ModuleFlagBehavior behavior, string name, UInt32 value )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            behavior.ThrowIfNotDefined();
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            var metadata = Context.CreateConstant( value ).ToMetadata();

            LLVMAddModuleFlag( ModuleHandle!, ( LLVMModuleFlagBehavior )behavior, name, name.Length, metadata.MetadataHandle );
        }

        /// <summary>Adds a module flag to the module</summary>
        /// <param name="behavior">Module flag behavior for this flag</param>
        /// <param name="name">Name of the flag</param>
        /// <param name="value">Value of the flag</param>
        public void AddModuleFlag( ModuleFlagBehavior behavior, string name, LlvmMetadata value )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            behavior.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( value );
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            LLVMAddModuleFlag( ModuleHandle!, ( LLVMModuleFlagBehavior )behavior, name, name.Length, value.MetadataHandle );
        }

        /// <summary>Adds operand value to named metadata</summary>
        /// <param name="name">Name of the metadata</param>
        /// <param name="value">operand value</param>
        public void AddNamedMetadataOperand( string name, LlvmMetadata value )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentNullException.ThrowIfNull( value );
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            LibLLVMAddNamedMetadataOperand2( ModuleHandle!, name, value?.MetadataHandle ?? default );
        }

        /// <summary>Adds an llvm.ident metadata string to the module</summary>
        /// <param name="version">version information to place in the llvm.ident metadata</param>
        public void AddVersionIdentMetadata( string version )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace(version);

            var stringNode = Context.CreateMDNode( version );
            LibLLVMAddNamedMetadataOperand2( ModuleHandle!, "llvm.ident", stringNode.MetadataHandle );
        }

        /// <summary>Creates a Function definition with Debug information</summary>
        /// <param name="scope">Containing scope for the function</param>
        /// <param name="name">Name of the function in source language form</param>
        /// <param name="linkageName">Mangled linker visible name of the function (may be same as <paramref name="name"/> if mangling not required by source language</param>
        /// <param name="file">File containing the function definition</param>
        /// <param name="line">Line number of the function definition</param>
        /// <param name="signature">LLVM Function type for the signature of the function</param>
        /// <param name="isLocalToUnit">Flag to indicate if this function is local to the compilation unit</param>
        /// <param name="isDefinition">Flag to indicate if this is a definition</param>
        /// <param name="scopeLine">First line of the function's outermost scope, this may not be the same as the first line of the function definition due to source formatting</param>
        /// <param name="debugFlags">Additional flags describing this function</param>
        /// <param name="isOptimized">Flag to indicate if this function is optimized</param>
        /// <returns>Function described by the arguments</returns>
        public Function CreateFunction( DIScope? scope
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
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            ArgumentNullException.ThrowIfNull( signature );
            if(string.IsNullOrWhiteSpace(linkageName))
            {
                linkageName = name;
            }

            if( signature.DebugInfoType == null )
            {
                throw new ArgumentException( Resources.Signature_requires_debug_type_information, nameof( signature ) );
            }

            var func = CreateFunction( linkageName, signature );
            var diSignature = signature.DebugInfoType;
            var diFunc = DIBuilder.CreateFunction( scope: scope
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

        /// <summary>Creates a function</summary>
        /// <param name="name">Name of the function</param>
        /// <param name="isVarArg">Flag indicating if the function supports a variadic argument list</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="argumentTypes">Arguments for the function</param>
        /// <returns>
        /// Function, matching the signature specified. This may be a previously declared or defined
        /// function or a new function if none matching the name and signature is already present.
        /// </returns>
        public Function CreateFunction( string name
                                        , bool isVarArg
                                        , IDebugType<ITypeRef, DIType> returnType
                                        , IEnumerable<IDebugType<ITypeRef, DIType>> argumentTypes
                                        )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            IFunctionType signature = Context.CreateFunctionType( DIBuilder, isVarArg, returnType, argumentTypes );
            return CreateFunction( name, signature );
        }

        /// <summary>Creates a function</summary>
        /// <param name="name">Name of the function</param>
        /// <param name="isVarArg">Flag indicating if the function supports a variadic argument list</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="argumentTypes">Arguments for the function</param>
        /// <returns>
        /// Function, matching the signature specified. This may be a previously declared or defined
        /// function or a new function if none matching the name and signature is already present.
        /// </returns>
        public Function CreateFunction( string name
                                        , bool isVarArg
                                        , IDebugType<ITypeRef, DIType> returnType
                                        , params IDebugType<ITypeRef, DIType>[ ] argumentTypes
                                        )
        {
            return CreateFunction( name, isVarArg, returnType, ( IEnumerable<IDebugType<ITypeRef, DIType>> )argumentTypes );
        }

        /// <summary>Gets a declaration for an LLVM intrinsic function</summary>
        /// <param name="name">Name of the intrinsic</param>
        /// <param name="args">Args for the intrinsic</param>
        /// <returns>Function declaration</returns>
        /// <remarks>
        /// This method will match overloaded intrinsics based on the parameter types. If an intrinsic
        /// has no overloads then an exact match is required. If the intrinsic has overloads than a prefix
        /// match is used.
        /// <note type="important">
        /// It is important to note that the prefix match requires the name provided to have a length greater
        /// than that of the name of the intrinsic and that the name starts with a matching overloaded intrinsic.
        /// for example: 'llvm.memset' would not match the overloaded memset intrinsic but 'llvm.memset.p.i' does.
        /// Thus, it is generally a good idea to use the signature from the LLVM documentation without the address
        /// space, or bit widths. That is instead of 'llvm.memset.p0i8.i32' use 'llvm.memset.p.i'.
        /// </note>
        /// </remarks>
        public Function GetIntrinsicDeclaration( string name, params ITypeRef[ ] args )
        {
            uint id = Intrinsic.LookupId( name );
            return GetIntrinsicDeclaration( id, args );
        }

        /// <summary>Gets a declaration for an LLVM intrinsic function</summary>
        /// <param name="id">id of the intrinsic</param>
        /// <param name="args">Arguments for the intrinsic</param>
        /// <returns>Function declaration</returns>
        public Function GetIntrinsicDeclaration( UInt32 id, params ITypeRef[ ] args )
        {
            ArgumentNullException.ThrowIfNull(args);
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            if( !LLVMIntrinsicIsOverloaded( id ) && args.Length > 0 )
            {
                throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Intrinsic_0_is_not_overloaded_and_therefore_does_not_require_type_arguments, id ) );
            }

            LLVMTypeRef[ ] llvmArgs = args.Select( a => a.GetTypeRef( ) ).ToArray( );
            LLVMValueRef valueRef = LLVMGetIntrinsicDeclaration( ModuleHandle!, id, llvmArgs, llvmArgs.Length );
            return ( Function )Value.FromHandle( valueRef.ThrowIfInvalid( ) )!;
        }

        /// <summary>Clones the current module</summary>
        /// <returns>Cloned module</returns>
        public BitcodeModule Clone( )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            return FromHandle( LLVMCloneModule( ModuleHandle! ).ThrowIfInvalid( ) )!;
        }

        /// <summary>Clones the module into a new <see cref="Context"/></summary>
        /// <param name="targetContext"><see cref="Context"/> to clone the module into</param>
        /// <returns>Cloned copy of the module</returns>
        public BitcodeModule Clone( Context targetContext )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentNullException.ThrowIfNull(targetContext);
            if( targetContext == Context )
            {
                return Clone( );
            }

            using var buffer = WriteToBuffer( );
            var retVal = LoadFrom( buffer, targetContext );
            Debug.Assert( retVal.Context == targetContext, Resources.Expected_to_get_a_module_bound_to_the_specified_context );
            return retVal;
        }

        /// <summary>Load a bit-code module from a given file</summary>
        /// <param name="path">path of the file to load</param>
        /// <param name="context">Context to use for creating the module</param>
        /// <returns>Loaded <see cref="BitcodeModule"/></returns>
        public static BitcodeModule LoadFrom( string path, Context context )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( path );
            ArgumentNullException.ThrowIfNull( context );

            if( !File.Exists( path ) )
            {
                throw new FileNotFoundException( Resources.Specified_bit_code_file_does_not_exist, path );
            }

            using var buffer = new MemoryBuffer( path );
            return LoadFrom( buffer, context );
        }

        /// <summary>Load bit code from a memory buffer</summary>
        /// <param name="buffer">Buffer to load from</param>
        /// <param name="context">Context to load the module into</param>
        /// <returns>Loaded <see cref="BitcodeModule"/></returns>
        /// <remarks>
        /// This along with <see cref="WriteToBuffer"/> are useful for "cloning"
        /// a module from one context to another. This allows creation of multiple
        /// modules on different threads and contexts and later moving them to a
        /// single context in order to link them into a single final module for
        /// optimization.
        /// </remarks>
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "Module created here is owned, and disposed of via the projected BitcodeModule" )]
        public static BitcodeModule LoadFrom( MemoryBuffer buffer, Context context )
        {
            ArgumentNullException.ThrowIfNull(buffer);
            ArgumentNullException.ThrowIfNull(context);

            return LLVMParseBitcodeInContext2( context.ContextHandle, buffer.Handle, out LLVMModuleRef modRef ).Failed
                ? throw new InternalCodeGeneratorException( Resources.Could_not_parse_bit_code_from_buffer )
                : context.GetModuleFor( modRef );
        }

        // Can be NULL if Disposed!
        internal LLVMModuleRef ModuleHandle { get; private set; }

        internal LLVMModuleRef Detach( )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            Context.RemoveModule( this );

            // MOVE ownership of the native handle to the return type
            return ModuleHandle.Move();
        }

        internal static BitcodeModule? FromHandle( LLVMModuleRef nativeHandle )
        {
            nativeHandle.ThrowIfInvalid();
            var contextRef = LLVMGetModuleContext( nativeHandle );
            Context context = ContextCache.GetContextFor( contextRef );
            return context.GetModuleFor( nativeHandle );
        }

        internal class InterningFactory
            : HandleInterningMapWithContext<LLVMModuleRef, BitcodeModule>
            , IBitcodeModuleFactory
        {
            public BitcodeModule CreateBitcodeModule( ) => CreateBitcodeModule( string.Empty );

            public BitcodeModule CreateBitcodeModule( string moduleId )
            {
                // empty string is OK.
                ArgumentNullException.ThrowIfNull( moduleId );

                var hContext = LLVMModuleCreateWithNameInContext( moduleId, Context.ContextHandle );
                return GetOrCreateItem( hContext );
            }

            public BitcodeModule CreateBitcodeModule( string moduleId
                                                    , SourceLanguage language
                                                    , string srcFilePath
                                                    , string producer
                                                    , bool optimized = false
                                                    , string compilationFlags = ""
                                                    , uint runtimeVersion = 0
                                                    )
            {
                var retVal = CreateBitcodeModule( moduleId );
                retVal.DICompileUnit = retVal.DIBuilder.CreateCompileUnit( language
                                                                         , srcFilePath
                                                                         , producer
                                                                         , optimized
                                                                         , compilationFlags
                                                                         , runtimeVersion
                                                                         );
                retVal.SourceFileName = Path.GetFileName( srcFilePath );
                return retVal;
            }

            internal InterningFactory( Context context )
                : base( context )
            {
            }

            private protected override BitcodeModule ItemFactory( LLVMModuleRef handle )
            {
                var contextRef = LLVMGetModuleContext( handle );
                if (!Context.ContextHandle.Equals(contextRef))
                {
                    handle.Dispose();
                    throw new ArgumentException( Resources.Context_mismatch_cannot_cache_modules_from_multiple_contexts );
                }

                return new BitcodeModule( handle );
            }
        }

        private BitcodeModule( LLVMModuleRef handle )
        {
            handle.ThrowIfInvalid();

            ModuleHandle = handle;
            Context = ContextCache.GetContextFor( LLVMGetModuleContext( handle ) );
            LazyDiBuilder = new Lazy<DebugInfoBuilder>( ( ) => new DebugInfoBuilder( this ) );
            Comdats = new ComdatCollection( this );
        }

        private readonly Lazy<DebugInfoBuilder> LazyDiBuilder;
    }
}
