// -----------------------------------------------------------------------
// <copyright file="IModule.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// until compiler bug [#40325](https://github.com/dotnet/roslyn/issues/40325) is resolved disable this
// as adding the parameter would result in 'warning: Duplicate parameter 'passes' found in comments, the latter one is ignored.'
// when generating the final documentation. (It picks the first one and reports a warning).
// Unfortunately there's no way to suppress this without cluttering up the code with pragmas.
// [ Sadly the SuppressMessageAttribute does NOT work for this warning. ]
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace Ubiquity.NET.Llvm
{
    /// <summary>Interface for an LLVM bitcode module</summary>
    /// <remarks>
    /// This interface is used for ALL modules owned/unowned when a module is owned
    /// and the app must dispose it a <see cref="Module"/> instance is provided. That
    /// type implements this interface AND the standard <see cref="IDisposable"/>
    /// interface as the caller is responsible for disposal of the instance. When
    /// this interface is returned the caller does not own the implementation and
    /// cannot dispose of it. (Generally only methods that act as a factory for a
    /// module will provide a concrete <see cref="Module"/> instance that the
    /// caller owns. Others are references to a module owned by the container)
    /// </remarks>
    public interface IModule
        : IEquatable<IModule>
    {
        /// <summary>Gets or sets the name of the source file generating this module</summary>
        public LazyEncodedString SourceFileName { get; set; }

        /// <summary>Gets the Comdats for this module</summary>
        public ComdatCollection Comdats { get; }

        /// <summary>Gets the <see cref="Context"/> this module belongs to</summary>
        public IContext Context { get; }

        /// <summary>Gets the IrMetadata for module level flags</summary>
        public IReadOnlyDictionary<LazyEncodedString, ModuleFlag> ModuleFlags { get; }

        /// <summary>Gets the Debug Compile units for this module</summary>
        public IEnumerable<DICompileUnit> CompileUnits { get; }

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
        public LazyEncodedString DataLayoutString { get; set; }

        /// <summary>Gets or sets the target data layout for this module</summary>
        /// <remarks>
        /// The setter uses a by value copy where the input value is serialized to
        /// a string and then set via <see cref="DataLayoutString"/>. That is, even
        /// if the implementation of <see cref="IDataLayout"/> is for an <see cref="IDisposable"/>
        /// type no ownership transfer occurs.
        /// </remarks>
        public IDataLayout Layout { get; set; }

        /// <summary>Gets or sets the Target Triple describing the target, ABI and OS</summary>
        public LazyEncodedString TargetTriple { get; set; }

        /// <summary>Gets the <see cref="GlobalVariable"/>s contained by this module</summary>
        public IEnumerable<GlobalVariable> Globals { get; }

        /// <summary>Gets the functions contained in this module</summary>
        public IEnumerable<Function> Functions { get; }

        /// <summary>Gets the global aliases in this module</summary>
        public IEnumerable<GlobalAlias> Aliases { get; }

        /// <summary>Gets the <see cref="NamedMDNode"/>s for this module</summary>
        public IEnumerable<NamedMDNode> NamedMetadata { get; }

        /// <summary>Gets the <see cref="GlobalIFunc"/>s in this module</summary>
        public IEnumerable<GlobalIFunc> IndirectFunctions { get; }

        /// <summary>Gets the name of the module</summary>
        public LazyEncodedString Name { get; }

        /// <summary>Gets or sets the module level inline assembly</summary>
        public LazyEncodedString ModuleInlineAsm { get; set; }

        /// <summary>Appends inline assembly to the module's inline assembly</summary>
        /// <param name="asm">assembly text</param>
        public void AppendInlineAsm( LazyEncodedString asm );

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
        public ErrorInfo TryRunPasses( params LazyEncodedString[] passes );

        /// <inheritdoc cref="TryRunPasses(LazyEncodedString[])"/>
        /// <param name="options">Options for the passes</param>
        public ErrorInfo TryRunPasses( PassBuilderOptions options, params LazyEncodedString[] passes );

        /// <inheritdoc cref="TryRunPasses(LazyEncodedString[])"/>
        /// <param name="targetMachine">Target machine for the passes</param>
        /// <param name="options">Options for the passes</param>
        public ErrorInfo TryRunPasses( TargetMachine targetMachine, PassBuilderOptions options, params LazyEncodedString[] passes );

        /// <summary>Link another module into this one</summary>
        /// <param name="srcModule">module to link into this one</param>
        /// <remarks>
        /// <note type="warning">
        /// <paramref name="srcModule"/> is destroyed by this process and no longer usable
        /// when this method returns.
        /// </note>
        /// </remarks>
        public void Link( Module srcModule );

        /// <summary>Verifies a bit-code module</summary>
        /// <param name="errorMessage">Error messages describing any issues found in the bit-code. Empty string if no error</param>
        /// <returns>true if the verification succeeded and false if not.</returns>
        public bool Verify( out string errorMessage );

        /// <summary>Looks up a function in the module by name</summary>
        /// <param name="name">Name of the function</param>
        /// <param name="function">The function or <see langword="null"/> if not found</param>
        /// <returns><see langword="true"/> if the function was found or <see langword="false"/> if not</returns>
        public bool TryGetFunction( LazyEncodedString name, [MaybeNullWhen( false )] out Function function );

        /// <summary>Create and add a global indirect function</summary>
        /// <param name="name">Name of the function</param>
        /// <param name="type">Signature of the function</param>
        /// <param name="addressSpace">Address space for the indirect function</param>
        /// <param name="resolver">Resolver for the indirect function</param>
        /// <returns>New <see cref="GlobalIFunc"/></returns>
        public GlobalIFunc CreateAndAddGlobalIFunc( LazyEncodedString name, ITypeRef type, uint addressSpace, Function resolver );

        /// <summary>Get a named Global Indirect function in the module</summary>
        /// <param name="name">Name of the ifunc to find</param>
        /// <param name="function">Function or <see langword="null"/> if not found</param>
        /// <returns><see langword="true"/> if the function was found or <see langword="false"/> if not</returns>
        public bool TryGetNamedGlobalIFunc( LazyEncodedString name, [MaybeNullWhen( false )] out GlobalIFunc function );

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
        public Function CreateFunction( LazyEncodedString name, IFunctionType signature );

        /// <summary>Writes a bit-code module to a file</summary>
        /// <param name="path">Path to write the bit-code into</param>
        /// <remarks>
        /// This is a blind write. (e.g. no verification is performed)
        /// So if an invalid module is saved it might not work with any
        /// later stage processing tools.
        /// </remarks>
        public void WriteToFile( string path );

        /// <summary>Writes this module as LLVM IR source to a file</summary>
        /// <param name="path">File to write the LLVM IR source to</param>
        /// <param name="errMsg">Error messages encountered, if any</param>
        /// <returns><see langword="true"/> if successful or <see langword="false"/> if not</returns>
        public bool WriteToTextFile( string path, out string errMsg );

        /// <summary>Creates a string representation of the module</summary>
        /// <returns>LLVM textual representation of the module</returns>
        /// <remarks>
        /// This is intentionally NOT an override of ToString() as that is
        /// used by debuggers to show the value of a type and this can take
        /// an extremely long time (up to many seconds depending on complexity
        /// of the module) which is usually bad for the debugger. If you need
        /// to see the contents of the IR for a module you can use this method
        /// in the immediate or watch windows.
        /// </remarks>
        public string? WriteToString( );

        /// <summary>Writes the LLVM IR bit code into a memory buffer</summary>
        /// <returns><see cref="MemoryBuffer"/> containing the bit code module</returns>
        public MemoryBuffer WriteToBuffer( );

        /// <summary>Add an alias to the module</summary>
        /// <param name="aliasee">Value being aliased</param>
        /// <param name="aliasName">Name of the alias</param>
        /// <param name="addressSpace">Address space for the alias [Default: 0]</param>
        /// <returns><see cref="GlobalAlias"/> for the alias</returns>
        public GlobalAlias AddAlias( Value aliasee, LazyEncodedString aliasName, uint addressSpace = 0 );

        /// <summary>Get an alias by name</summary>
        /// <param name="name">name of the alias to get</param>
        /// <returns>Alias matching <paramref name="name"/> or null if no such alias exists</returns>
        public GlobalAlias? GetAlias( LazyEncodedString name );

        /// <summary>Adds a global to this module with a specific address space</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="name">Name of the global</param>
        /// <returns>The new <see cref="GlobalVariable"/></returns>
        /// <openissues>
        /// - What does LLVM do if creating a second Global with the same name (return null, throw, crash??,...)
        /// </openissues>
        public GlobalVariable AddGlobalInAddressSpace( uint addressSpace, ITypeRef typeRef, LazyEncodedString name );

        /// <summary>Adds a global to this module</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobalInAddressSpace( uint addressSpace, ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal );

        /// <summary>Adds a global to this module</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <param name="name">Name of the variable</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobalInAddressSpace(
            uint addressSpace,
            ITypeRef typeRef,
            bool isConst,
            Linkage linkage,
            Constant constVal,
            LazyEncodedString name
            );

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="name">Name of the global</param>
        /// <returns>The new <see cref="GlobalVariable"/></returns>
        /// <openissues>
        /// - What does LLVM do if creating a second Global with the same name (return null, throw, crash??,...)
        /// </openissues>
        public GlobalVariable AddGlobal( ITypeRef typeRef, LazyEncodedString name );

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobal( ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal );

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <param name="name">Name of the variable</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobal( ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal, LazyEncodedString name );

        /// <summary>Retrieves a <see cref="ITypeRef"/> by name from the module</summary>
        /// <param name="name">Name of the type</param>
        /// <returns>The type or null if no type with the specified name exists in the module</returns>
        public ITypeRef? GetTypeByName( LazyEncodedString name );

        /// <summary>Retrieves a named global from the module</summary>
        /// <param name="name">Name of the global</param>
        /// <returns><see cref="GlobalVariable"/> or <see langword="null"/> if not found</returns>
        public GlobalVariable? GetNamedGlobal( LazyEncodedString name );

        /// <summary>Adds a module flag to the module</summary>
        /// <param name="behavior">ModuleHandle flag behavior for this flag</param>
        /// <param name="name">Name of the flag</param>
        /// <param name="value">Value of the flag</param>
        public void AddModuleFlag( ModuleFlagBehavior behavior, LazyEncodedString name, UInt32 value );

        /// <summary>Adds a module flag to the module</summary>
        /// <param name="behavior">ModuleHandle flag behavior for this flag</param>
        /// <param name="name">Name of the flag</param>
        /// <param name="value">Value of the flag</param>
        public void AddModuleFlag( ModuleFlagBehavior behavior, LazyEncodedString name, IrMetadata value );

        /// <summary>Adds operand value to named metadata</summary>
        /// <param name="name">Name of the metadata</param>
        /// <param name="value">operand value</param>
        public void AddNamedMetadataOperand( LazyEncodedString name, IrMetadata value );

        /// <summary>Adds an llvm.ident metadata string to the module</summary>
        /// <param name="version">version information to place in the llvm.ident metadata</param>
        public void AddVersionIdentMetadata( LazyEncodedString version );

        /// <summary>Creates a Function definition with Debug information</summary>
        /// <param name="diBuilder">The debug info builder to use to create the function (must be associated with this module)</param>
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
        public Function CreateFunction( ref readonly DIBuilder diBuilder
                                      , DIScope? scope
                                      , LazyEncodedString name
                                      , LazyEncodedString? linkageName
                                      , DIFile? file
                                      , uint line
                                      , DebugFunctionType signature
                                      , bool isLocalToUnit
                                      , bool isDefinition
                                      , uint scopeLine
                                      , DebugInfoFlags debugFlags
                                      , bool isOptimized
                                      );

        /// <summary>Creates a function</summary>
        /// <param name="diBuilder"><see cref="DIBuilder"/> for creation of debug information</param>
        /// <param name="name">Name of the function</param>
        /// <param name="isVarArg">Flag indicating if the function supports a variadic argument list</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="argumentTypes">Arguments for the function</param>
        /// <returns>
        /// Function, matching the signature specified. This may be a previously declared or defined
        /// function or a new function if none matching the name and signature is already present.
        /// </returns>
        public Function CreateFunction( ref readonly DIBuilder diBuilder
                                      , LazyEncodedString name
                                      , bool isVarArg
                                      , IDebugType<ITypeRef, DIType> returnType
                                      , IEnumerable<IDebugType<ITypeRef, DIType>> argumentTypes
                                      );

        /// <summary>Creates a function</summary>
        /// <param name="diBuilder"><see cref="DIBuilder"/> for creation of debug information</param>
        /// <param name="name">Name of the function</param>
        /// <param name="isVarArg">Flag indicating if the function supports a variadic argument list</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="argumentTypes">Arguments for the function</param>
        /// <returns>
        /// Function, matching the signature specified. This may be a previously declared or defined
        /// function or a new function if none matching the name and signature is already present.
        /// </returns>
        public Function CreateFunction( ref readonly DIBuilder diBuilder
                                      , LazyEncodedString name
                                      , bool isVarArg
                                      , IDebugType<ITypeRef, DIType> returnType
                                      , params IDebugType<ITypeRef, DIType>[] argumentTypes
                                      );

        /// <summary>Gets a declaration for an LLVM intrinsic function</summary>
        /// <param name="name">Name of the intrinsic</param>
        /// <param name="args">Args for the intrinsic</param>
        /// <returns>Function declaration</returns>
        /// <remarks>
        /// This method will match an overloaded intrinsic based on the parameter types. If an intrinsic
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
        public Function GetIntrinsicDeclaration( LazyEncodedString name, params ITypeRef[] args );

        /// <summary>Gets a declaration for an LLVM intrinsic function</summary>
        /// <param name="id">id of the intrinsic</param>
        /// <param name="args">Arguments for the intrinsic</param>
        /// <returns>Function declaration</returns>
        public Function GetIntrinsicDeclaration( UInt32 id, params ITypeRef[] args );

        /// <summary>Clones the current module into the same context</summary>
        /// <returns>Cloned module</returns>
        public Module Clone( );

        /// <summary>Clones the module into a new <see cref="IContext"/></summary>
        /// <param name="targetContext"><see cref="IContext"/> to clone the module into</param>
        /// <returns>Cloned copy of the module</returns>
        public Module Clone( IContext targetContext );
    }

    // Internal helper to get the underlying ABI handle as an "Alias" handle
    // even when it is an owned module. This hides the difference between
    // the two.
    internal static class ModuleExtensions
    {
        internal static LLVMModuleRefAlias GetUnownedHandle( this IModule self )
        {
            if(self is IHandleWrapper<LLVMModuleRefAlias> wrapper)
            {
                return wrapper.Handle;
            }
            else if(self is IGlobalHandleOwner<LLVMModuleRef> owner)
            {
                // implicitly cast to the unowned alias handle
                // ownership is retained by "self"
                return owner.OwnedHandle;
            }
            else
            {
                throw new ArgumentException( "Internal Error - Unknown module type!", nameof( self ) );
            }
        }

        // Convenience accessor extension method to resolve ugly casting
        // to generic interface and make semantic intent clear.
        internal static LLVMModuleRef GetOwnedHandle( this IGlobalHandleOwner<LLVMModuleRef> owner )
        {
            return owner.OwnedHandle;
        }

        // Convenience accessor extension method to resolve ugly casting
        // to generic interface and make semantic intent clear.
        internal static void InvalidateFromMove( this IGlobalHandleOwner<LLVMModuleRef> owner )
        {
            owner.InvalidateFromMove();
        }
    }
}
