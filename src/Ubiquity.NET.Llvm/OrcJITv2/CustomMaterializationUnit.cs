// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Delegate to perform action on Materialization</summary>
    /// <param name="r"><see cref="MaterializationResponsibility"/> that serves as the context for this materialization</param>
    /// <remarks>
    /// This must be a "custom" delegate as the <see cref="JITDyLib"/> is a
    /// ref type that is NOT allowed as a type parameter for <see cref="Action{T1, T2}"/>.
    /// </remarks>
    public delegate void MaterializationAction(MaterializationResponsibility r);

    /// <summary>Delegate to perform action on discard</summary>
    /// <param name="jitLib">Library the symbols is discarded from</param>
    /// <param name="symbol">Symbol being discarded</param>
    /// <remarks>
    /// This must be a "custom" delegate as the <see cref="JITDyLib"/> is a
    /// ref type that is NOT allowed as a type parameter for <see cref="Action{T1, T2}"/>.
    /// </remarks>
    public delegate void DiscardAction(JITDyLib jitLib, SymbolStringPoolEntry symbol);

    /// <summary>LLVM ORC JIT v2 custom materialization unit</summary>
    /// <remarks>
    /// This is used for the bulk of "Lazy" JIT support. However, it is important to
    /// note that this class does not (and cannot) retain any instance data. All
    /// data used by the actual materialization is owned by the provided delegates or the
    /// <see cref="IDisposable"/> implementation.
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
            IReadOnlyDictionary<SymbolStringPoolEntry, SymbolFlags> symbols,
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
        /// until the materialization is completed or abandoned. If <paramref name="materializeAction"/> is not called, then
        /// the <paramref name="discardAction"/> is. This allows for cases where the data used by the <paramref name="materializeAction"/>
        /// is not held by the action itself or needs to support early disposal instead of remaining at the whims of the GC.
        /// </remarks>
        public CustomMaterializationUnit(
            LazyEncodedString name,
            MaterializationAction materializeAction,
            DiscardAction? discardAction,
            IReadOnlyDictionary<SymbolStringPoolEntry, SymbolFlags> symbols,
            SymbolStringPoolEntry? initSymbol = null
            )
            : base( MakeHandle( name, symbols, materializeAction, discardAction, initSymbol ) )
        {
        }

        // Provides construction of a materialization unit handle for the base type
        private static LLVMOrcMaterializationUnitRef MakeHandle(
            LazyEncodedString name,
            IReadOnlyDictionary<SymbolStringPoolEntry, SymbolFlags> symbols,
            MaterializationAction materializeAction,
            DiscardAction? discardAction,
            SymbolStringPoolEntry? initSymbol = null
            )
        {
            ArgumentNullException.ThrowIfNull( name );
            ArgumentNullException.ThrowIfNull( materializeAction );

            // This will internally manage the lifetime
            using var materializer = new CustomMaterializer(materializeAction, discardAction);
            unsafe
            {
                using var nativeSyms = InitializeNativeCopy(symbols);
                using var pinnedSyms = nativeSyms.Memory.Pin();

                // Bump ref count for the native code to "own", it does NOT do this on it's
                // own and instead assumes caller owns and "moves" the ref count responsibility.
                initSymbol?.DangerousAddRef();
                nint nativeContext = materializer.AddRefAndGetNativeContext();
                try
                {
                    using MemoryHandle nativeMem = name.Pin();
                    return LLVMOrcCreateCustomMaterializationUnit(
                        (byte*)nativeMem.Pointer,
                        (void*)nativeContext,
                        (LLVMOrcCSymbolFlagsMapPair*)pinnedSyms.Pointer,
                        symbols.Count,
                        initSymbol?.Handle ?? LLVMOrcSymbolStringPoolEntryRef.Zero,
                        &NativeCallbacks.Materialize,
                        materializer.SupportsDiscard ? &NativeCallbacks.Discard : null,
                        &NativeCallbacks.Destroy
                        );
                }
                catch
                {
                    // in the unlikely event of an exception; restore the ref count so it doesn't leak
                    initSymbol?.DangerousRelease();
                    materializer.Dispose();
                    throw;
                }
            }
        }

        // static class to provide the native callbacks for custom materialization
        // These all assume the managed delegates are held within an instance of CustomMaterializer
        private static class NativeCallbacks
        {
            [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            internal static unsafe void Materialize(void* context, /*LLVMOrcMaterializationResponsibilityRef*/ nint abiResponsibility)
            {
                try
                {
                    if(context is not null && GCHandle.FromIntPtr( (nint)context ).Target is CustomMaterializer self)
                    {
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                        // [It is an alias; Dispose is a NOP with wasted overhead]
                        self.MaterializeHandler( new MaterializationResponsibility( abiResponsibility, alias: true ) );
#pragma warning restore IDISP004 // Don't ignore created IDisposable
#pragma warning restore CA2000 // Dispose objects before losing scope

#pragma warning disable IDISP007 // Don't dispose injected
                        /*
                        Not really "injected" and this is how the data/context is disposed when the
                        native code is done with it.
                        */
                        self.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected
                    }
                }
                catch
                {
                    Debug.Assert( false, "Exception in native callback!" );
                }
            }

            [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            internal static unsafe void Discard(void* context, /*LLVMOrcJITDylibRef*/ nint abiLib, /*LLVMOrcSymbolStringPoolEntryRef*/ nint abiSymbol)
            {
                try
                {
                    if(context is not null && GCHandle.FromIntPtr( (nint)context ).Target is CustomMaterializer self)
                    {
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                        // [It is an alias; Dispose is a NOP with wasted overhead]
                        self.DiscardHandler?.Invoke( new JITDyLib( abiLib ), new SymbolStringPoolEntry( abiSymbol, alias: true ) );
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
            internal static unsafe void Destroy(void* context)
            {
                try
                {
                    if(context is not null && GCHandle.FromIntPtr( (nint)context ).Target is CustomMaterializer self)
                    {
#pragma warning disable IDISP007 // Don't dispose injected
                        /*
                        Not really "injected" and this is how the data/context is disposed when the
                        native code is done with it.
                        */
                        self.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected
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
