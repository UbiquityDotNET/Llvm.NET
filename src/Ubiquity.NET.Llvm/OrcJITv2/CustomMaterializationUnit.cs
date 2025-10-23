// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Delegate to perform action on Materialization</summary>
    /// <param name="r"><see cref="MaterializationResponsibility"/> that serves as the context for this materialization</param>
    /// <remarks>
    /// This is "custom" delegate for consistency with <see cref="DiscardAction"/>
    /// which requires one for .NET runtimes lower than .NET 9.
    /// </remarks>
    public delegate void MaterializationAction( MaterializationResponsibility r );

    /// <summary>Delegate to perform action on discard</summary>
    /// <param name="jitLib">Library the symbols is discarded from</param>
    /// <param name="symbol">Symbol being discarded</param>
    /// <remarks>
    /// This must be a "custom" delegate as the <see cref="JITDyLib"/> is a
    /// ref type that is NOT allowed as a type parameter for <see cref="Action{T1, T2}"/>.
    /// </remarks>
    public delegate void DiscardAction( ref readonly JITDyLib jitLib, SymbolStringPoolEntry symbol );

    /// <summary>LLVM ORC JIT v2 custom materialization unit</summary>
    /// <remarks>
    /// This is used for the bulk of "Lazy" JIT support. However, it is important to
    /// note that this class does not (and cannot) retain any instance data. All
    /// data used by the actual materialization is owned by the provided delegates.
    /// </remarks>
    public sealed class CustomMaterializationUnit
        : MaterializationUnit
    {
        /// <summary>Initializes a new instance of the <see cref="CustomMaterializationUnit"/> class.</summary>
        /// <param name="name">Name of this instance</param>
        /// <param name="materializeAction">Action to perform to materialize the symbol</param>
        /// <param name="symbols">symbols the materializer works on</param>
        /// <param name="initSymbol">Symbol of static initializer (if any)</param>
        public CustomMaterializationUnit(
            LazyEncodedString name,
            MaterializationAction materializeAction,
            ImmutableArray<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols,
            SymbolStringPoolEntry? initSymbol = null
            )
            : base( MakeHandle( name, symbols, materializeAction, null, initSymbol ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CustomMaterializationUnit"/> class.</summary>
        /// <param name="name">Name of this instance</param>
        /// <param name="materializeAction">Action to perform to materialize the symbol</param>
        /// <param name="discardAction">Action to perform when the JIT discards/replaces a symbol</param>
        /// <param name="symbols">symbols the materializer works on</param>
        /// <param name="initSymbol">Symbol of static initializer (if any)</param>
        /// <remarks>
        /// This implementation will maintain the lifetime of the provided delegates via an internally allocated disposable
        /// until the materialization is completed or abandoned. If <paramref Name="materializeAction"/> is not called, then
        /// the <paramref Name="discardAction"/> is. This allows for cases where the data used by the <paramref Name="materializeAction"/>
        /// is not held by the action itself or needs to support early disposal instead of remaining at the whims of the GC.
        /// <note type="important">
        /// If <paramref Name="initSymbol"/> is not <see langword="null"/> it <b><em>MUST</em></b> have
        /// <see cref="SymbolGenericOption.MaterializationSideEffectsOnly"/> set AND this implementation takes ownership of
        /// the ref count for this symbol (Move semantics). If callers want access to the string beyond this call they must
        /// increment the ref count <b><em>before</em></b> calling this function.
        /// </note>
        /// </remarks>
        public CustomMaterializationUnit(
            LazyEncodedString name,
            MaterializationAction materializeAction,
            DiscardAction? discardAction,
            ImmutableArray<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols,
            SymbolStringPoolEntry? initSymbol = null
            )
            : base( MakeHandle( name, symbols, materializeAction, discardAction, initSymbol ) )
        {
        }

        // Provides construction of a materialization unit handle for the base type
        private static LLVMOrcMaterializationUnitRef MakeHandle(
            LazyEncodedString name,
            ImmutableArray<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols,
            MaterializationAction materializeAction,
            DiscardAction? discardAction,
            SymbolStringPoolEntry? initSymbol = null
            )
        {
            ArgumentNullException.ThrowIfNull( name );
            ArgumentNullException.ThrowIfNull( materializeAction );
            ValidateInitSym(initSymbol, symbols);
            LLVMOrcMaterializationUnitRef retVal;

            using IMemoryOwner<LLVMOrcCSymbolFlagsMapPair> nativeSyms = symbols.InitializeNativeCopy();

            // using expression ensures cleanup of addref in case of exceptions...
            // But since it is an immutable Value type that isn't viable here and try/finally is used
            LLVMOrcSymbolStringPoolEntryRef nativeInitSym = initSymbol?.AddRefForNative() ?? default;
            unsafe
            {
                void* nativeContext = null; // init only from inside try/catch block
                try
                {
                    nativeContext = new MaterializerCallbacksHolder(materializeAction, discardAction).AsNativeContext();
                    using var pinnedSyms = nativeSyms.Memory.Pin();
                    retVal = LLVMOrcCreateCustomMaterializationUnit(
                        name,
                        nativeContext,
                        (LLVMOrcCSymbolFlagsMapPair*)pinnedSyms.Pointer,
                        checked((nuint)symbols.Length),
                        nativeInitSym,
                        &NativeCallbacks.Materialize,
                        &NativeCallbacks.Discard,
                        &NativeCallbacks.Destroy
                        );

                    // ownership of this symbol was moved to native, mark transfer so auto clean up (for exceptional cases)
                    // does NOT kick in.
                    nativeInitSym = default;
                }
                catch when (nativeContext is not null)
                {
                    // release the handle allocated for the native code as it isn't used there in the face of an exception here
                    NativeContext.Release( ref nativeContext);
                    throw;
                }
                finally
                {
                    if(!nativeInitSym.IsNull)
                    {
                        nativeInitSym.Dispose();
                    }
                }
            }

            return retVal;
        }

        [Conditional( "DEBUG" )]
        private static void ValidateInitSym(
            SymbolStringPoolEntry? initSym,
            ImmutableArray<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols
            )
        {
            if(initSym is not null && !initSym.IsDisposed && symbols.Any( kvp => kvp.Key.Name.Equals( initSym.Name ) ))
            {
                throw new KeyNotFoundException($"Symbol '{initSym.Name} not found in '{nameof(symbols)}'");
            }
        }

        // static class to provide the native callbacks for custom materialization
        // These all assume the managed delegates are held within an instance of CustomMaterializer
        private static class NativeCallbacks
        {
            [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            internal static unsafe void Materialize( void* context, /*LLVMOrcMaterializationResponsibilityRef*/ nint abiResponsibility )
            {
                try
                {
                    if(NativeContext.TryFrom<IMaterializerCallbacks>(context, out var self ))
                    {
                        // Destroy callback is NOT called if this one is...
                        // Internally LLVM will set the context to null [Undocumented!]
                        NativeContext.Release(context);

#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                        // [It is an alias; Dispose is a NOP with wasted overhead]
                        self.Materialize( new MaterializationResponsibility( abiResponsibility, alias: true ) );
#pragma warning restore IDISP004 // Don't ignore created IDisposable
#pragma warning restore CA2000 // Dispose objects before losing scope
                    }
                }
                catch
                {
                    Debug.Assert( false, "Exception in native callback!" );
                }
            }

            [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            internal static unsafe void Discard( void* context, /*LLVMOrcJITDylibRef*/ nint abiLib, /*LLVMOrcSymbolStringPoolEntryRef*/ nint abiSymbol )
            {
                try
                {
                    if(NativeContext.TryFrom<IMaterializerCallbacks>( context, out var self ))
                    {
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                        // [It is an alias; Dispose is a NOP with wasted overhead]
                        var managedLib = new JITDyLib( abiLib );
                        self.Discard( in managedLib, new SymbolStringPoolEntry( abiSymbol, alias: true ) );
#pragma warning restore IDISP004 // Don't ignore created IDisposable
#pragma warning restore CA2000 // Dispose objects before losing scope
                    }
                }
                catch
                {
                    Debug.Assert( false, "Exception in native callback!" );
                }
            }

            [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            internal static unsafe void Destroy( void* context )
            {
                try
                {
                    if(NativeContext.TryFrom<IMaterializerCallbacks>(context, out var self ))
                    {
                        // self is a managed instance with normal GC rules now so release
                        // the context created for callbacks as it is not needed anymore.
                        // After this scope exits, GC is free to collect the instance.
                        NativeContext.Release(context);
                        self.Destroy();
                    }
                }
                catch
                {
                    Debug.Assert( false, "Exception in native callback!" );
                }
            }
        }
    }
}
