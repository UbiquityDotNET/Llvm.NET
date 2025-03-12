// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 custom materialization unit</summary>
    /// <remarks>
    /// This is used for the bulk of "Lazy" JIT support. However, it is important to
    /// note that this class does not (and cannot) retain any instance data. All
    /// data used by the actual materialization is owned by the <see cref="CustomMaterializer"/>
    /// provided. The instance provided is Disposed as early as possible by the native callbacks.
    /// Thus, if multiple symbols materializers share the same method to perform the materialization
    /// then each one needs a distinct <see cref="CustomMaterializer"/> instance to ensure the resource
    /// ownership is retained correctly.
    /// </remarks>
    public sealed class CustomMaterializationUnit
        : MaterializationUnit
    {
        /// <summary>Initializes a new instance of the <see cref="CustomMaterializationUnit"/> class.</summary>
        /// <param name="name">Name of this instance</param>
        /// <param name="materializer">Materializer to hold the call back delegates to perform the actual materialization work</param>
        /// <param name="symbols">symbols the materializer works on</param>
        /// <param name="initSymbol">Symbol of static initializer (if any)</param>
        /// <remarks>
        /// This implementation will call <see cref="IDisposable.Dispose"/> on the provided <see cref="CustomMaterializer"/>
        /// after it calls the Materialize action  or after it calls the Destroy action [Only one of those two is ever called and ONLY once].
        /// This is done to ensure that the resources needed for materialization are released as early as possible.
        /// </remarks>
        public CustomMaterializationUnit(
            LazyEncodedString name,
            CustomMaterializer materializer,
            IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols,
            SymbolStringPoolEntry? initSymbol = null
            )
            : base( MakeHandle( name, symbols, materializer, initSymbol ) )
        {
        }

        private static LLVMOrcMaterializationUnitRef MakeHandle(
            LazyEncodedString name,
            IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols,
            CustomMaterializer materializer,
            SymbolStringPoolEntry? initSymbol = null
            )
        {
            ArgumentNullException.ThrowIfNull( name );
            ArgumentNullException.ThrowIfNull( materializer );

            unsafe
            {
                using var nativeSyms = InitializeNativeCopy(symbols);
                using var pinnedSyms = nativeSyms.Memory.Pin();

                // Bump ref count for the native code to "own", it does NOT do this on it's
                // own and instead assumes caller owns and "moves" the ref count responsibility.
                initSymbol?.DangerousAddRef();
                materializer.AddRef();
                try
                {
                    using MemoryHandle nativeMem = name.Pin();
                    return LLVMOrcCreateCustomMaterializationUnit(
                        (byte*)nativeMem.Pointer,
                        materializer.GetNativeContext(),
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
                        // [It is an alias; Dispose is a NOP with wasted overhead]
                        self.MaterializeHandler( new MaterializationResponsibility( abiResponsibility, alias: true ) );
#pragma warning restore CA2000 // Dispose objects before losing scope
                        self.Dispose();
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
                        // [It is an alias; Dispose is a NOP with wasted overhead]
                        self.DiscardHandler?.Invoke( new JITDyLib( abiLib ), new SymbolStringPoolEntry( abiSymbol, alias: true ) );
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
                        self.DestroyHandler();
                        self.Dispose();
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
