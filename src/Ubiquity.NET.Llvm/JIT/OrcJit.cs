// -----------------------------------------------------------------------
// <copyright file="OrcJit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.JIT
{
    /// <summary>LLVM On Request Compilation (ORC) Just-In-Time (JIT) Engine</summary>
    /// <remarks>
    /// The LLVM OrcJIT supports lazy compilation and better resource management for
    /// clients. For more details on the implementation see the LLVM Documentation.
    /// </remarks>
    [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "multiple levels of the ternary conditional expression is anything but a simplification" )]
    public class OrcJit
        : DisposableObject
        , ILazyCompileExecutionEngine
    {
        /// <summary>Initializes a new instance of the <see cref="OrcJit"/> class for a given target machine.</summary>
        /// <param name="machine">Target machine for the JIT</param>
        public OrcJit( TargetMachine machine )
        {
            machine.ValidateNotNull( nameof( machine ) );

            JitStackHandle = LLVMOrcCreateInstance( machine.TargetMachineHandle ).ThrowIfInvalid();
            TargetMachine = machine;
        }

        /// <inheritdoc/>
        public TargetMachine TargetMachine { get; }

        /// <inheritdoc/>
        public ulong AddEagerlyCompiledModule( BitcodeModule bitcodeModule, LLVMOrcSymbolResolverFn resolver )
        {
            bitcodeModule.ValidateNotNull( nameof( bitcodeModule ) );
            resolver.ValidateNotNull( nameof( resolver ) );

            // detach the module before providing to JIT as JIT takes ownership
            LLVMModuleRef moduleHandle = bitcodeModule.Detach( );
            var wrappedResolver = new WrappedNativeCallback<LLVMOrcSymbolResolverFn>( resolver );
            var err = LLVMOrcAddEagerlyCompiledIR( JitStackHandle, out ulong retHandle, moduleHandle, wrappedResolver, IntPtr.Zero );
            moduleHandle.SetHandleAsInvalid( );
            if( !err.IsInvalid )
            {
                throw new LlvmException( err.ToString( ) );
            }

            // keep resolver delegate alive as native code needs to call it after this function exits
            SymbolResolvers.Add( retHandle, wrappedResolver );
            return retHandle;
        }

        /// <inheritdoc/>
        public ulong AddLazyCompiledModule( BitcodeModule bitcodeModule, LLVMOrcSymbolResolverFn resolver )
        {
            bitcodeModule.ValidateNotNull( nameof( bitcodeModule ) );
            resolver.ValidateNotNull( nameof( resolver ) );

            LLVMModuleRef moduleHandle = bitcodeModule.Detach( );
            var wrappedResolver = new WrappedNativeCallback<LLVMOrcSymbolResolverFn>( resolver );
            var err = LLVMOrcAddLazilyCompiledIR( JitStackHandle, out ulong retHandle, moduleHandle, wrappedResolver, IntPtr.Zero );
            moduleHandle.SetHandleAsInvalid( );
            if( !err.IsInvalid )
            {
                throw new LlvmException( err.ToString( ) );
            }

            // keep resolver delegate alive as native code needs to call it after this function exits
            SymbolResolvers.Add( retHandle, wrappedResolver );
            return retHandle;
        }

        /// <inheritdoc/>
        public void RemoveModule( ulong handle )
        {
            var err = LLVMOrcRemoveModule( JitStackHandle, handle );
            if( !err.IsInvalid )
            {
                throw new LlvmException( err.ToString( ) );
            }

            SymbolResolvers.Remove( handle );
        }

        /// <inheritdoc/>
        public ulong DefaultSymbolResolver( string name, IntPtr ctx )
        {
            try
            {
                // Workaround OrcJit+Windows/COFF bug/limitation where functions in the generated obj are
                // not marked as exported, so the official llvm-c API doesn't see the symbol to get the address
                // LibLLVM variant takes the additional bool so that is used on Windows to find non-exported symbols.
                bool exportedOnly = Environment.OSVersion.Platform != PlatformID.Win32NT;
                var err = LibLLVMOrcGetSymbolAddress( JitStackHandle, out ulong retAddr, name, exportedOnly );
                if( !err.IsInvalid )
                {
                    throw new InvalidOperationException( string.Format( CultureInfo.CurrentCulture, Resources.Unresolved_Symbol_0_1, name, LLVMOrcGetErrorMsg( JitStackHandle ) ) );
                }

                if( retAddr != 0)
                {
                    return retAddr;
                }

                return GlobalInteropFunctions.TryGetValue( name, out WrappedNativeCallback? callBack ) ? ( ulong )callBack.ToIntPtr( ).ToInt64( ) : 0;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                // Allowing exceptions outside this call is not helpful as the LLVM
                // native JIT engine is what calls this function and it doesn't know
                // how to deal with a managed exception. Any exceptions are at least
                // logged in a debugger before being swallowed here.
                return 0;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <inheritdoc/>
        public T GetFunctionDelegate<T>( string name )
        {
            // Workaround OrcJit+Windows/COFF bug/limitation where functions in the generated obj are
            // not marked as exported, so the official llvm-c API doesn't see the symbol to get the address
            // LibLLVM variant takes the additional bool so that is used on Windows to find non-exported symbols.
            bool exportedOnly = Environment.OSVersion.Platform != PlatformID.Win32NT;
            var err = LibLLVMOrcGetSymbolAddress( JitStackHandle, out UInt64 retAddr, name, exportedOnly );
            if( !err.IsInvalid )
            {
                throw new LlvmException( err.ToString( ) );
            }

            if( retAddr == 0 )
            {
                throw new KeyNotFoundException( string.Format( CultureInfo.CurrentCulture, Resources.Function_0_not_found, name ) );
            }

            return Marshal.GetDelegateForFunctionPointer<T>( ( IntPtr )retAddr );
        }

        /// <summary>Adds or replaces an interop callback for a global symbol</summary>
        /// <typeparam name="T">Delegate type for the callback</typeparam>
        /// <param name="symbolName">Symbol name of the global</param>
        /// <param name="delegate">Delegate for the callback</param>
        /// <remarks>
        /// <note type="warning">
        /// The delegate is made available to native code as a callback, and therefore it
        /// must have a lifetime that is at least as long as the callback is registered or
        /// the lifetime of the JIT engine itself. The direct delegate and any instance
        /// it may be a member of are handled automatically in the internal implementation
        /// of this function. However, any data the delegate may rely on is not. (e.g. if
        /// the object the delegate is a method on a class implementing IDisposable and the
        /// Dispose method was called on that instance, then the callback could end up operating
        /// on a disposed object)
        /// </note>
        /// <note type="warning">
        /// The callback **MUST NOT** throw any exceptions out of the callback, as the
        /// JIT engine doesn't know how to handle them and neither does the JIT'd code.
        /// </note>
        /// </remarks>
        public void AddInteropCallback<T>( string symbolName, T @delegate )
            where T : Delegate
        {
            LLVMOrcGetMangledSymbol( JitStackHandle, out string mangledName, symbolName );
            if( GlobalInteropFunctions.TryGetValue( mangledName, out WrappedNativeCallback? existingCallback ) )
            {
                GlobalInteropFunctions.Remove( mangledName );
                existingCallback.Dispose( );
            }

            GlobalInteropFunctions.Add( mangledName, new WrappedNativeCallback<T>( @delegate ) );
        }

// LLVM 10 is transitioning to ORC JIT v2 and unfortunately,
// the lazy function generator is doing it's own symbol resolution that doesn't
// account for the COFF export bug
#if LAZY_FUNCTION_GENERATOR_SUPPORTED
        /// <inheritdoc/>
        public void AddLazyFunctionGenerator( string name, LazyFunctionCompiler generator, IntPtr context )
        {
            LLVMOrcGetMangledSymbol( JitStackHandle, out string mangledName, name );

            // wrap the provided generator function for a safe native callback
            UInt64 CompileAction( LLVMOrcJITStackRef jitStackHandle, IntPtr ctx )
            {
                try
                {
                    (string implName, BitcodeModule module) = generator( );
                    if( module == null )
                    {
                        return 0;
                    }

                    AddEagerlyCompiledModule( module, DefaultSymbolResolver );

                    // Workaround OrcJit+Windows/COFF bug/limitation where functions in the generated obj are
                    // not marked as exported, so the official llvm-c API doesn't see the symbol to get the address
                    // LibLLVM variant takes the additional bool so that is used on Windows to find non-exported symbols.
                    bool exportedOnly = Environment.OSVersion.Platform != PlatformID.Win32NT;
                    var e = LibLLVMOrcGetSymbolAddress( JitStackHandle, out UInt64 implAddr, name, exportedOnly );
                    if( !e.IsInvalid )
                    {
                        throw new LlvmException( e.ToString( ) );
                    }

                    e = LLVMOrcSetIndirectStubPointer( JitStackHandle, mangledName, implAddr );
                    if( !e.IsInvalid )
                    {
                        throw new LlvmException( e.ToString( ) );
                    }

                    LazyFunctionGenerators.Remove( mangledName );
                    return implAddr;
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch
                {
                    // native callback - MUST NOT leak exceptions out of this call.
                    System.Diagnostics.Debug.Assert( false, "Callbacks should not allow exceptions - this is an application crash scenario!" );
                    return 0;
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }

            var callbackAction = new WrappedNativeCallback<LLVMOrcLazyCompileCallbackFn>( CompileAction );
            LazyFunctionGenerators.Add( mangledName, callbackAction );

            var err = LLVMOrcCreateLazyCompileCallback( JitStackHandle, out UInt64 stubAddr, callbackAction, context );
            if( !err.IsInvalid )
            {
                throw new LlvmException( err.ToString( ) );
            }

            err = LLVMOrcCreateIndirectStub( JitStackHandle, mangledName, stubAddr );
            if( !err.IsInvalid )
            {
                throw new LlvmException( err.ToString( ) );
            }
        }
#endif

        /// <summary>Releases unmanaged resources for this instance</summary>
        /// <param name="disposing">Indicates if this is from a Dispose (<see langword="true"/>) or a finalizer</param>
        protected override void Dispose( bool disposing )
        {
            JitStackHandle?.Dispose( );
            DisposeCallbacks( GlobalInteropFunctions );
            DisposeCallbacks( SymbolResolvers );
            DisposeCallbacks( LazyFunctionGenerators );
        }

        private static void DisposeCallbacks<T, T2>( IDictionary<T, T2> map )
            where T2 : WrappedNativeCallback
        {
            if( map != null )
            {
                foreach( var callBack in map.Values )
                {
                    callBack.Dispose( );
                }

                map.Clear( );
            }
        }

        private readonly Dictionary<string, WrappedNativeCallback> GlobalInteropFunctions = new();

        private readonly Dictionary<UInt64, WrappedNativeCallback<LLVMOrcSymbolResolverFn>> SymbolResolvers = new();

        private readonly Dictionary<string, WrappedNativeCallback<LLVMOrcLazyCompileCallbackFn>> LazyFunctionGenerators = new();

        private readonly LLVMOrcJITStackRef JitStackHandle;
    }
}
