// <copyright file="Module.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Llvm.NET.DebugInfo;
using Llvm.NET.Native;
using Llvm.NET.Native.Handles;
using Llvm.NET.Types;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Enumeration to indicate the behavior of module level flags metadata sharing the same name in a <see cref="BitcodeModule"/></summary>
    [SuppressMessage( "Microsoft.Naming"
                    , "CA1726:UsePreferredTerms"
                    , MessageId = "Flag"
                    , Justification = "Enum for the behavior of the LLVM ModuleFlag (Flag in middle doesn't imply the enum is Bit Flags)" )]
    public enum ModuleFlagBehavior
    {
        /// <summary>Invalid value (default value for this enumeration)</summary>
        Invalid = 0,

        /// <summary>Emits an error if two values disagree, otherwise the resulting value is that of the operands</summary>
        Error = LLVMModFlagBehavior.Error,

        /// <summary>Emits a warning if two values disagree. The result will be the operand for the flag from the first module being linked</summary>
        Warning = LLVMModFlagBehavior.Warning,

        /// <summary>Adds a requirement that another module flag be present and have a specified value after linking is performed</summary>
        /// <remarks>
        /// The value must be a metadata pair, where the first element of the pair is the ID of the module flag to be restricted, and the
        /// second element of the pair is the value the module flag should be restricted to. This behavior can be used to restrict the
        /// allowable results (via triggering of an error) of linking IDs with the <see cref="Override"/> behavior
        /// </remarks>
        Require = LLVMModFlagBehavior.Require,

        /// <summary>Uses the specified value, regardless of the behavior or value of the other module</summary>
        /// <remarks>If both modules specify Override, but the values differ, and error will be emitted</remarks>
        Override = LLVMModFlagBehavior.Override,

        /// <summary>Appends the two values, which are required to be metadata nodes</summary>
        Append = LLVMModFlagBehavior.Append,

        /// <summary>Appends the two values, which are required to be metadata nodes dropping duplicate entries in the second list</summary>
        AppendUnique = LLVMModFlagBehavior.AppendUnique,

        /// <summary>Takes the max of the two values, which are required to be integers</summary>
        Max = 7 /*LLVMModFlagBehavior.Max*/
    }

    /// <summary>LLVM Bit-code module</summary>
    /// <remarks>
    /// A module is the basic unit for containing code in LLVM. Modules are an in memory
    /// representation of the LLVM bit-code.
    /// </remarks>
    public sealed class BitcodeModule
        : IDisposable
        , IExtensiblePropertyContainer
    {
        /// <summary>Initializes a new instance of the <see cref="BitcodeModule"/> class in a given context</summary>
        /// <param name="context">Context for the module</param>
        public BitcodeModule( [NotNull] Context context )
            : this( context, string.Empty )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BitcodeModule"/> class in a given context</summary>
        /// <param name="moduleId">Module's ID</param>
        /// <param name="context">Context for the module</param>
        public BitcodeModule( [NotNull] Context context, string moduleId )
        {
            moduleId.ValidateNotNull( nameof(moduleId) );
            ModuleHandle = LLVMModuleCreateWithNameInContext( moduleId, context.ContextHandle );
            if( ModuleHandle == default )
            {
                throw new InternalCodeGeneratorException( "Could not create module in context" );
            }

            LazyDiBuilder = new Lazy<DebugInfoBuilder>( ( ) => new DebugInfoBuilder( this ) );
            Context.AddModule( this );
            Comdats = new ComdatCollection( this );
        }

        /// <summary>Initializes a new instance of the <see cref="BitcodeModule"/> class with a root <see cref="DICompileUnit"/> to contain debugging information</summary>
        /// <param name="context">Context for the module</param>
        /// <param name="moduleId">Module name</param>
        /// <param name="language">Language to store in the debugging information</param>
        /// <param name="srcFilePath">path of source file to set for the compilation unit</param>
        /// <param name="producer">Name of the application producing this module</param>
        /// <param name="optimized">Flag to indicate if the module is optimized</param>
        /// <param name="compilationFlags">Additional flags</param>
        /// <param name="runtimeVersion">Runtime version if any (use 0 if the runtime version has no meaning)</param>
        public BitcodeModule( Context context
                            , string moduleId
                            , SourceLanguage language
                            , string srcFilePath
                            , string producer
                            , bool optimized = false
                            , string compilationFlags = ""
                            , uint runtimeVersion = 0
                            )
            : this( context, moduleId )
        {
            DICompileUnit = DIBuilder.CreateCompileUnit( language
                                                       , srcFilePath
                                                       , producer
                                                       , optimized
                                                       , compilationFlags
                                                       , runtimeVersion
                                                       );

            SourceFileName = Path.GetFileName( srcFilePath );
        }

        /// <summary>Gets a value indicating whether the module is diposed or not</summary>
        public bool IsDisposed => ModuleHandle == default;

        /// <summary>Gets or sets the name of the source file generating this module</summary>
        public string SourceFileName
        {
            get
            {
                ValidateHandle( );
                return LLVMGetModuleSourceFileName( ModuleHandle );
            }

            set
            {
                ValidateHandle( );
                LLVMSetModuleSourceFileName( ModuleHandle, value );
            }
        }

        /// <summary>Name of the Debug Version information module flag</summary>
        public const string DebugVersionValue = "Debug Info Version";

        /// <summary>Name of the Dwarf Version module flag</summary>
        public const string DwarfVersionValue = "Dwarf Version";

        /// <summary>Version of the Debug information Metadata</summary>
        public const UInt32 DebugMetadataVersion = 3; /* DEBUG_METADATA_VERSION (for LLVM v3.7.0) */

        /// <summary>Gets the Comdats for this module</summary>
        public ComdatCollection Comdats { get; }

        /// <summary>Gets the <see cref="Context"/> this module belongs to</summary>
        public Context Context
        {
            get
            {
                ValidateHandle( );
                if( ModuleHandle == default )
                {
                    return null;
                }

                return ModuleHandle.Context;
            }
        }

        /// <summary>Gets the Metadata for module level flags</summary>
        public IReadOnlyDictionary<string, ModuleFlag> ModuleFlags
        {
            get
            {
                ValidateHandle( );
                var handle = LLVMModuleGetModuleFlagsMetadata( ModuleHandle );
                if( handle == default )
                {
                    return new Dictionary<string, ModuleFlag>( );
                }

                var namedNode = new NamedMDNode( handle );
                return namedNode.Operands
                                .Select( n => new ModuleFlag( n ) )
                                .ToDictionary( f => f.Name );
            }
        }

        /// <summary>Gets the <see cref="DebugInfoBuilder"/> used to create debug information for this module</summary>
        public DebugInfoBuilder DIBuilder
        {
            get
            {
                ValidateHandle( );
                return LazyDiBuilder.Value;
            }
        }

        /// <summary>Gets the Debug Compile unit for this module</summary>
        public DICompileUnit DICompileUnit { get; internal set; }

        /// <summary>Gets the Data layout string for this module</summary>
        /// <remarks>
        /// <note type="note">The data layout string doesn't do what seems obvious.
        /// That is, it doesn't force the target back-end to generate code
        /// or types with a particular layout. Rather, the layout string has
        /// to match the implicit layout of the target. Thus it should only
        /// come from the actual <see cref="TargetMachine"/> the code is
        /// targeting.</note>
        /// </remarks>
        public string DataLayoutString
        {
            get
            {
                ValidateHandle( );
                return Layout?.ToString( ) ?? string.Empty;
            }
        }

        /// <summary>Gets or sets the target data layout for this module</summary>
        public DataLayout Layout
        {
            get
            {
                ValidateHandle( );
                return Layout_;
            }

            set
            {
                ValidateHandle( );

                Layout_ = value;
                LLVMSetDataLayout( ModuleHandle, value?.ToString( ) ?? string.Empty );
            }
        }

        /// <summary>Gets or sets the Target Triple describing the target, ABI and OS</summary>
        public string TargetTriple
        {
            get
            {
                ValidateHandle( );
                return LLVMGetTarget( ModuleHandle );
            }

            set
            {
                ValidateHandle( );
                LLVMSetTarget( ModuleHandle, value );
            }
        }

        /// <summary>Gets the Globals contained by this module</summary>
        public IEnumerable<Value> Globals
        {
            get
            {
                ValidateHandle( );
                var current = LLVMGetFirstGlobal( ModuleHandle );
                while( current != default )
                {
                    yield return Value.FromHandle( current );
                    current = LLVMGetNextGlobal( current );
                }
            }
        }

        /// <summary>Gets the functions contained in this module</summary>
        public IEnumerable<Function> Functions
        {
            get
            {
                ValidateHandle( );
                var current = LLVMGetFirstFunction( ModuleHandle );
                while( current != default )
                {
                    yield return Value.FromHandle<Function>( current );
                    current = LLVMGetNextFunction( current );
                }
            }
        }

        // TODO: Add enumerator for GlobalAlias(s)
        // TODO: Add enumerator for NamedMDNode(s)

        /// <summary>Gets the name of the module</summary>
        public string Name
        {
            get
            {
                ValidateHandle( );
                return LLVMGetModuleName( ModuleHandle );
            }
        }

        /// <summary>Gets a value indicating whether this module is shared with a JIT engine</summary>
        public bool IsShared => SharedModuleRef != null;

        /// <summary>Makes this module a shared module if it isn't already</summary>
        /// <remarks>
        /// Normally, this is called automatically when adding a module to a JIT engine that needs shared modules.
        /// Shared modules are reference counted so they aren't completely disposed until the reference count
        /// is decremented to 0. Calling <see cref="Dispose()"/> on a shared module will mark the module as disposed
        /// and decrement the share count.
        /// </remarks>
        public void MakeShared( )
        {
            if( !IsShared )
            {
                SharedModuleRef = LLVMOrcMakeSharedModule( ModuleHandle );
            }
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
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
            ValidateHandle( );
            otherModule.ValidateNotNull( nameof( otherModule ) );

            if( otherModule.Context != Context )
            {
                throw new ArgumentException( "Linking modules from different contexts is not allowed", nameof( otherModule ) );
            }

            if( !LLVMLinkModules2( ModuleHandle, otherModule.ModuleHandle ).Succeeded )
            {
                throw new InternalCodeGeneratorException( "Module link error" );
            }

            Context.RemoveModule( otherModule );
            otherModule.ModuleHandle = new LLVMModuleRef( IntPtr.Zero );
        }

        /// <summary>Verifies a bit-code module</summary>
        /// <param name="errmsg">Error messages describing any issues found in the bit-code</param>
        /// <returns>true if the verification succeeded and false if not.</returns>
        public bool Verify( out string errmsg )
        {
            ValidateHandle( );
            return LLVMVerifyModule( ModuleHandle, LLVMVerifierFailureAction.LLVMReturnStatusAction, out errmsg ).Succeeded;
        }

        /// <summary>Gets a function by name from this module</summary>
        /// <param name="name">Name of the function to get</param>
        /// <returns>The function or null if not found</returns>
        public Function GetFunction( string name )
        {
            ValidateHandle( );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            var funcRef = LLVMGetNamedFunction( ModuleHandle, name );
            if( funcRef == default )
            {
                return null;
            }

            return Value.FromHandle<Function>( funcRef );
        }

        /// <summary>Add a function with the specified signature to the module</summary>
        /// <param name="name">Name of the function to add</param>
        /// <param name="signature">Signature of the function</param>
        /// <returns><see cref="Function"/>matching the specified signature and name</returns>
        /// <remarks>
        /// If a matching function already exists it is returned, and therefore the returned
        /// <see cref="Function"/> may have a body and additional attributes. If a function of
        /// the same name exists with a different signature an exception is thrown as LLVM does
        /// not perform any function overloading.
        /// </remarks>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public Function AddFunction( string name, IFunctionType signature )
        {
            ValidateHandle( );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            signature.ValidateNotNull( nameof( signature ) );

            return Value.FromHandle<Function>( LLVMGetOrInsertFunction( ModuleHandle, name, signature.GetTypeRef( ) ) );
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
            ValidateHandle( );
            path.ValidateNotNullOrWhiteSpace( nameof( path ) );

            LLVMStatus status = LLVMWriteBitcodeToFile( ModuleHandle, path );
            if( status.Failed )
            {
                throw new IOException( $"Error writing bit-code file '{path}'", status.ErrorCode );
            }
        }

        /// <summary>Writes this module as LLVM IR source to a file</summary>
        /// <param name="path">File to write the LLVM IR source to</param>
        /// <param name="errMsg">Error messages encountered, if any</param>
        /// <returns><see langword="true"/> if successful or <see langword="false"/> if not</returns>
        public bool WriteToTextFile( string path, out string errMsg )
        {
            ValidateHandle( );
            path.ValidateNotNullOrWhiteSpace( nameof( path ) );

            return LLVMPrintModuleToFile( ModuleHandle, path, out errMsg ).Succeeded;
        }

        /// <summary>Creates a string representation of the module</summary>
        /// <returns>LLVM textual representation of the module</returns>
        /// <remarks>
        /// This is intentionally NOT an override of ToString() as that is
        /// used by debuggers to show the value of a type and this can take
        /// an extremely long time (up to many seconds depending on complexity
        /// of the module) which is bad for the debugger.
        /// </remarks>
        public string WriteToString( )
        {
            ValidateHandle( );
            return LLVMPrintModuleToString( ModuleHandle );
        }

        /// <summary>Writes the LLVM IR bit code into a memory buffer</summary>
        /// <returns><see cref="MemoryBuffer"/> containing the bit code module</returns>
        public MemoryBuffer WriteToBuffer( )
        {
            ValidateHandle( );
            return new MemoryBuffer( LLVMWriteBitcodeToMemoryBuffer( ModuleHandle ) );
        }

        /// <summary>Add an alias to the module</summary>
        /// <param name="aliasee">Value being aliased</param>
        /// <param name="aliasName">Name of the alias</param>
        /// <returns><see cref="GlobalAlias"/> for the alias</returns>
        public GlobalAlias AddAlias( Value aliasee, string aliasName )
        {
            ValidateHandle( );
            aliasee.ValidateNotNull( nameof( aliasee ) );
            aliasName.ValidateNotNullOrWhiteSpace( nameof( aliasName ) );

            var handle = LLVMAddAlias( ModuleHandle, aliasee.NativeType.GetTypeRef( ), aliasee.ValueHandle, aliasName );
            return Value.FromHandle<GlobalAlias>( handle );
        }

        /// <summary>Get an alias by name</summary>
        /// <param name="name">name of the alias to get</param>
        /// <returns>Alias matching <paramref name="name"/> or null if no such alias exists</returns>
        public GlobalAlias GetAlias( string name )
        {
            ValidateHandle( );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            var handle = LLVMGetGlobalAlias( ModuleHandle, name );
            return Value.FromHandle<GlobalAlias>( handle );
        }

        /// <summary>Adds a global to this module with a specific address space</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the global's value</param>
        /// <param name="name">Name of the global</param>
        /// <returns>The new <see cref="GlobalVariable"/></returns>
        /// <openissues>
        /// - What does LLVM do if creating a second Global with the same name (return null, throw, crash??,...)
        /// </openissues>
        public GlobalVariable AddGlobalInAddressSpace( uint addressSpace, ITypeRef typeRef, string name )
        {
            ValidateHandle( );
            typeRef.ValidateNotNull( nameof( typeRef ) );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            var handle = LLVMAddGlobalInAddressSpace( ModuleHandle, typeRef.GetTypeRef( ), name, addressSpace );
            return Value.FromHandle<GlobalVariable>( handle );
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the global's value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobalInAddressSpace( uint addressSpace, ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal )
        {
            ValidateHandle( );
            typeRef.ValidateNotNull( nameof( typeRef ) );
            linkage.ValidateDefined( nameof( linkage ) );
            constVal.ValidateNotNull( nameof( constVal ) );

            return AddGlobalInAddressSpace( addressSpace, typeRef, isConst, linkage, constVal, string.Empty );
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="addressSpace">Address space to add the global to</param>
        /// <param name="typeRef">Type of the global's value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <param name="name">Name of the variable</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobalInAddressSpace( uint addressSpace, ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal, string name )
        {
            ValidateHandle( );
            typeRef.ValidateNotNull( nameof( typeRef ) );
            linkage.ValidateDefined( nameof( linkage ) );
            constVal.ValidateNotNull( nameof( constVal ) );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            var retVal = AddGlobalInAddressSpace( addressSpace, typeRef, name );
            retVal.IsConstant = isConst;
            retVal.Linkage = linkage;
            retVal.Initializer = constVal;
            return retVal;
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the global's value</param>
        /// <param name="name">Name of the global</param>
        /// <returns>The new <see cref="GlobalVariable"/></returns>
        /// <openissues>
        /// - What does LLVM do if creating a second Global with the same name (return null, throw, crash??,...)
        /// </openissues>
        public GlobalVariable AddGlobal( ITypeRef typeRef, string name )
        {
            ValidateHandle( );
            typeRef.ValidateNotNull( nameof( typeRef ) );
            name.ValidateNotNull( nameof( name ) );

            var handle = LLVMAddGlobal( ModuleHandle, typeRef.GetTypeRef( ), name );
            return Value.FromHandle<GlobalVariable>( handle );
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the global's value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobal( ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal )
        {
            ValidateHandle( );
            typeRef.ValidateNotNull( nameof( typeRef ) );
            linkage.ValidateDefined( nameof( linkage ) );
            constVal.ValidateNotNull( nameof( constVal ) );

            return AddGlobal( typeRef, isConst, linkage, constVal, string.Empty );
        }

        /// <summary>Adds a global to this module</summary>
        /// <param name="typeRef">Type of the global's value</param>
        /// <param name="isConst">Flag to indicate if this global is a constant</param>
        /// <param name="linkage">Linkage type for this global</param>
        /// <param name="constVal">Initial value for the global</param>
        /// <param name="name">Name of the variable</param>
        /// <returns>New global variable</returns>
        public GlobalVariable AddGlobal( ITypeRef typeRef, bool isConst, Linkage linkage, Constant constVal, string name )
        {
            ValidateHandle( );
            typeRef.ValidateNotNull( nameof( typeRef ) );
            linkage.ValidateDefined( nameof( linkage ) );
            constVal.ValidateNotNull( nameof( constVal ) );
            name.ValidateNotNull( nameof( name ) );

            var retVal = AddGlobal( typeRef, name );
            retVal.IsConstant = isConst;
            retVal.Linkage = linkage;
            retVal.Initializer = constVal;
            return retVal;
        }

        /// <summary>Retrieves a <see cref="ITypeRef"/> by name from the module</summary>
        /// <param name="name">Name of the type</param>
        /// <returns>The type or null if no type with the specified name exists in the module</returns>
        public ITypeRef GetTypeByName( string name )
        {
            ValidateHandle( );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            var hType = LLVMGetTypeByName( ModuleHandle, name );
            return hType == default ? null : TypeRef.FromHandle( hType );
        }

        /// <summary>Retrieves a named global from the module</summary>
        /// <param name="name">Name of the global</param>
        /// <returns><see cref="GlobalVariable"/> or <see langword="null"/> if not found</returns>
        public GlobalVariable GetNamedGlobal( string name )
        {
            ValidateHandle( );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            var hGlobal = LLVMGetNamedGlobal( ModuleHandle, name );
            if( hGlobal == default )
            {
                return null;
            }

            return Value.FromHandle<GlobalVariable>( hGlobal );
        }

        /// <summary>Adds a module flag to the module</summary>
        /// <param name="behavior">Module flag behavior for this flag</param>
        /// <param name="name">Name of the flag</param>
        /// <param name="value">Value of the flag</param>
        public void AddModuleFlag( ModuleFlagBehavior behavior, string name, UInt32 value )
        {
            ValidateHandle( );
            behavior.ValidateDefined( nameof( behavior ) );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            // AddModuleFlag comes from custom LLVMDebug-C API
            LLVMAddModuleFlag( ModuleHandle, ( LLVMModFlagBehavior )behavior, name, value );
        }

        /// <summary>Adds a module flag to the module</summary>
        /// <param name="behavior">Module flag behavior for this flag</param>
        /// <param name="name">Name of the flag</param>
        /// <param name="value">Value of the flag</param>
        public void AddModuleFlag( ModuleFlagBehavior behavior, string name, LlvmMetadata value )
        {
            ValidateHandle( );
            behavior.ValidateDefined( nameof( behavior ) );
            value.ValidateNotNull( nameof( value ) );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            // AddModuleFlag comes from custom LLVMDebug-C API
            LLVMAddModuleFlag( ModuleHandle, ( LLVMModFlagBehavior )behavior, name, value.MetadataHandle );
        }

        /// <summary>Adds operand value to named metadata</summary>
        /// <param name="name">Name of the metadata</param>
        /// <param name="value">operand value</param>
        public void AddNamedMetadataOperand( string name, LlvmMetadata value )
        {
            ValidateHandle( );
            value.ValidateNotNull( nameof( value ) );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            LLVMAddNamedMetadataOperand2( ModuleHandle, name, value?.MetadataHandle ?? default );
        }

        /// <summary>Adds an llvm.ident metadata string to the module</summary>
        /// <param name="version">version information to place in the llvm.ident metadata</param>
        public void AddVersionIdentMetadata( string version )
        {
            ValidateHandle( );
            version.ValidateNotNullOrWhiteSpace( nameof( version ) );

            var stringNode = CreateMDNode( version );
            LLVMAddNamedMetadataOperand2( ModuleHandle, "llvm.ident", stringNode.MetadataHandle );
        }

        /// <summary>Create an <see cref="MDNode"/> from a string</summary>
        /// <param name="value">String value</param>
        /// <returns>New node with the string as the first element of the <see cref="MDNode.Operands"/> property (as an MDString)</returns>
        public MDNode CreateMDNode( string value )
        {
            ValidateHandle( );
            var elements = new LLVMMetadataRef[ ] { LLVMMDString2( Context.ContextHandle, value, ( uint )( value?.Length ?? 0 ) ) };
            var hNode = LLVMMDNode2( Context.ContextHandle, out elements[ 0 ], ( uint )elements.Length );
            return MDNode.FromHandle<MDNode>( hNode );
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
        /// <param name="tParam">Parameter Metadata node</param>
        /// <param name="decl">Declaration Metadata node</param>
        /// <returns>Function described by the arguments</returns>
        public Function CreateFunction( DIScope scope
                                      , string name
                                      , string linkageName
                                      , DIFile file
                                      , uint line
                                      , DebugFunctionType signature
                                      , bool isLocalToUnit
                                      , bool isDefinition
                                      , uint scopeLine
                                      , DebugInfoFlags debugFlags
                                      , bool isOptimized
                                      , [CanBeNull] MDNode tParam = null
                                      , [CanBeNull] MDNode decl = null
                                      )
        {
            ValidateHandle( );
            scope.ValidateNotNull( nameof( scope ) );
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            signature.ValidateNotNull( nameof( signature ) );
            if( signature.DIType == null )
            {
                throw new ArgumentException( "Signature requires debug type information", nameof( signature ) );
            }

            var func = AddFunction( linkageName ?? name, signature );
            var diSignature = signature.DIType;
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
                                                 , typeParameter: tParam
                                                 , declaration: decl
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
            ValidateHandle( );
            IFunctionType signature = Context.CreateFunctionType( DIBuilder, isVarArg, returnType, argumentTypes );
            return AddFunction( name, signature );
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

        /// <summary>Clones the current module</summary>
        /// <returns>Cloned module</returns>
        public BitcodeModule Clone( )
        {
            ValidateHandle( );
            return new BitcodeModule( LLVMCloneModule( ModuleHandle ) );
        }

        /// <summary>Clones the module into a new <see cref="Context"/></summary>
        /// <param name="targetContext"><see cref="Context"/> to clone the module into</param>
        /// <returns>Cloned copy of the module</returns>
        public BitcodeModule Clone( Context targetContext )
        {
            ValidateHandle( );
            targetContext.ValidateNotNull( nameof( targetContext ) );

            if( targetContext == Context )
            {
                return Clone( );
            }

            using( var buffer = WriteToBuffer( ) )
            {
                var retVal = LoadFrom( buffer, targetContext );
                Debug.Assert( retVal.Context == targetContext, "Expected to get a module bound to the specified context" );
                return retVal;
            }
        }

        /// <inheritdoc/>
        bool IExtensiblePropertyContainer.TryGetExtendedPropertyValue<T>( string id, out T value )
            => PropertyBag.TryGetExtendedPropertyValue( id, out value );

        /// <inheritdoc/>
        void IExtensiblePropertyContainer.AddExtendedPropertyValue( string id, object value )
            => PropertyBag.AddExtendedPropertyValue( id, value );

        /// <summary>Load a bit-code module from a given file</summary>
        /// <param name="path">path of the file to load</param>
        /// <param name="context">Context to use for creating the module</param>
        /// <returns>Loaded <see cref="BitcodeModule"/></returns>
        public static BitcodeModule LoadFrom( string path, Context context )
        {
            path.ValidateNotNullOrWhiteSpace( nameof( path ) );
            context.ValidateNotNull( nameof( context ) );

            if( !File.Exists( path ) )
            {
                throw new FileNotFoundException( "Specified bit-code file does not exist", path );
            }

            using( var buffer = new MemoryBuffer( path ) )
            {
                return LoadFrom( buffer, context );
            }
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
        public static BitcodeModule LoadFrom( MemoryBuffer buffer, Context context )
        {
            buffer.ValidateNotNull( nameof( buffer ) );
            context.ValidateNotNull( nameof( context ) );

            if( LLVMParseBitcodeInContext2( context.ContextHandle, buffer.BufferHandle, out LLVMModuleRef modRef ).Failed )
            {
                throw new InternalCodeGeneratorException( "Could not parse bit code from buffer" );
            }

            return context.GetModuleFor( modRef );
        }

        internal BitcodeModule( LLVMModuleRef handle )
        {
            handle.ValidateNotDefault( nameof( handle ) );

            ModuleHandle = handle;
            LazyDiBuilder = new Lazy<DebugInfoBuilder>( ( ) => new DebugInfoBuilder( this ) );
            Context.AddModule( this );
            Comdats = new ComdatCollection( this );
        }

        internal LLVMModuleRef ModuleHandle { get; private set; }

        internal LLVMSharedModuleRef SharedModuleRef { get; private set; }

        internal LLVMModuleRef Detach( )
        {
            Context.RemoveModule( this );
            var retVal = ModuleHandle;
            ModuleHandle = default;
            return retVal;
        }

        internal static BitcodeModule FromHandle( LLVMModuleRef nativeHandle )
        {
            nativeHandle.ValidateNotDefault( nameof( nativeHandle ) );
            var context = nativeHandle.Context;
            return context.GetModuleFor( nativeHandle );
        }

        private void ValidateHandle( )
        {
            if( IsDisposed )
            {
                throw new ObjectDisposedException( "Module was explicitly destroyed or ownership transfered to native library" );
            }
        }

        private void Dispose( bool disposing )
        {
            // if not already disposed, dispose the module
            // Do this only on dispose. The containing context
            // will clean up the module when it is disposed or
            // finalized. Since finalization order isn't
            // deterministic it is possible that the module is
            // finalized after the context has already run its
            // finalizer, which would cause an access violation
            // in the native LLVM layer.
            if( disposing && ModuleHandle != default )
            {
                // remove the module handle from the module cache.
                Context.RemoveModule( this );

                // if this module was shared with a JIT, just release
                // the ref-count but don't dispose the actual module
                if( IsShared )
                {
                    SharedModuleRef.Close( );
                    SharedModuleRef = default;
                }
                else
                {
                    LLVMDisposeModule( ModuleHandle );
                }

                ModuleHandle = default;
            }
        }

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private static extern LLVMSharedModuleRef LLVMOrcMakeSharedModule( LLVMModuleRef Mod );

        // TODO: Leverage a generic WriteOnce<T> of some sort to enforce intentions in code rather than "by convention"
        [SuppressMessage( "StyleCop.CSharp.NamingRules"
                        , "SA1310:Field names must not contain underscore"
                        , Justification = "Trailing _ indicates it must not be written to directly even directly"
                        )
        ]
        private DataLayout Layout_;

        private readonly ExtensiblePropertyContainer PropertyBag = new ExtensiblePropertyContainer( );
        private readonly Lazy<DebugInfoBuilder> LazyDiBuilder;
    }
}
