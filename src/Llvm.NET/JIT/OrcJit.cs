// <copyright file="OrcJit.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Llvm.NET.Interop;
using Llvm.NET.Properties;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.JIT
{
    /// <summary>LLVM On Request Compilation (ORC) Just-In-Time (JIT) Engine</summary>
    /// <remarks>
    /// The LLVM OrcJIT supports lazy compilation and better resource management for
    /// clients. For more details on the implementation see the LLVM Documentation.
    /// </remarks>
    public class OrcJit
        : DisposableObject
        , ILazyCompileExecutionEngine
    {
        /// <summary>Initializes a new instance of the <see cref="OrcJit"/> class for a given target machine.</summary>
        /// <param name="machine">Target machine for the JIT</param>
        public OrcJit( TargetMachine machine )
        {
            JitStackHandle = LLVMOrcCreateInstance( machine.TargetMachineHandle );
            TargetMachine = machine;
        }

        /// <inheritdoc/>
        public TargetMachine TargetMachine { get; }

        /// <summary>Add a module to the engine</summary>
        /// <param name="module">The module to add to the engine</param>
        /// <returns>Handle for the module in the engine</returns>
        /// <remarks>
        /// <note type="note">
        /// With <see cref="OrcJit"/> the module is shared with the engine using a reference
        /// count. In this case the module is not disposed and the <see cref="BitcodeModule.IsShared"/>
        /// property is set to <see langword="true"/>. Callers may continue to use the module in this case,
        /// though modifying it or interned data from it's context may result in undefined behavior.
        /// </note>
        /// </remarks>
        public IJitModuleHandle AddModule( BitcodeModule module )
        {
            return ( JitModuleHandle<UInt64> )AddModule( module, DefaultSymbolResolver );
        }

        /// <summary>Add a module to the engine</summary>
        /// <param name="module">The module to add to the engine</param>
        /// <param name="resolver">Symbol resolver delegate</param>
        /// <returns>Handle for the module in the engine</returns>
        /// <remarks>
        /// <note type="warning">
        /// The <paramref name="resolver"/> must not throw an exception as the native LLVM JIT engine
        /// won't understand it and would leave the engine and LLVM in an inconsistent state. If the
        /// symbol isn't found LLVM generates an error message in debug builds and in all builds, terminates
        /// the application.
        /// </note>
        /// </remarks>
        public IJitModuleHandle AddModule( BitcodeModule module, SymbolResolver resolver )
        {
            module.MakeShared( );
            var wrappedResolver = new WrappedNativeCallback( resolver );
#if LLVM_COFF_EXPORT_BUG_FIXED
/* see: https://reviews.llvm.org/rL258665 */
            var err = LLVMOrcAddEagerlyCompiledIR( JitStackHandle, out UInt64 retHandle, module.SharedModuleRef, wrappedResolver.NativeFuncPtr, IntPtr.Zero );
#else
            // symbols are resolved if lazy compiled, requesting the address looks up the symbol in the IR module
            // where the COFF bug doesn't get in the way. The function is then JIT compiled to produce a native
            // function and the address of that function is returned.
            var err = LLVMOrcAddLazilyCompiledIR( JitStackHandle, out UInt64 retHandle, module.ModuleHandle, wrappedResolver.NativeFuncPtr, IntPtr.Zero );
#endif
            if( err != null )
            {
                throw new LlvmException( err.ToString( ) );
            }

            // keep resolver delegate alive as native code needs to call it after this function exits
            SymbolResolvers.Add( retHandle, wrappedResolver );
            return ( JitModuleHandle<UInt64> )retHandle;
        }

        /// <inheritdoc/>
        public void RemoveModule( IJitModuleHandle handle )
        {
            if( !( handle is JitModuleHandle<UInt64> orcHandle ) )
            {
                throw new ArgumentException( Resources.Invalid_handle_provided, nameof( handle ) );
            }

            var err = LLVMOrcRemoveModule( JitStackHandle, orcHandle );
            if( err != null )
            {
                throw new LlvmException( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            SymbolResolvers.Remove( orcHandle );
        }

        /// <inheritdoc/>
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "Native callback function *MUST NOT* surface managed exceptions" )]
        public UInt64 DefaultSymbolResolver( string name, IntPtr ctx )
        {
            try
            {
                var err = LLVMOrcGetSymbolAddress( JitStackHandle, out UInt64 retAddr, name );
                if( err != null )
                {
                    throw new InvalidOperationException( string.Format( Resources.Unresolved_Symbol_0_1, name, LLVMOrcGetErrorMsg( JitStackHandle ) ) );
                }

                if( retAddr != 0 )
                {
                    return retAddr;
                }

                if( GlobalInteropFunctions.TryGetValue( name, out WrappedNativeCallback callBack ) )
                {
                    return ( UInt64 )callBack.NativeFuncPtr.ToInt64( );
                }

                return 0;
            }
            catch
            {
                // Allowing exceptions outside this call is not helpful as the LLVM
                // native JIT engine is what calls this function and it doesn't know
                // how to deal with a managed exception. Any exceptions are at least
                // logged in a debugger before being swallowed here.
                return 0;
            }
        }

        /// <inheritdoc/>
        public T GetFunctionDelegate<T>( string name )
        {
            var err = LLVMOrcGetSymbolAddress( JitStackHandle, out UInt64 retAddr, name );
            if( err != null )
            {
                throw new LlvmException( err.ToString( ) );
            }

            if( retAddr == 0 )
            {
                throw new KeyNotFoundException( string.Format( Resources.Function_0_not_found, name ) );
            }

            return Marshal.GetDelegateForFunctionPointer<T>( ( IntPtr )retAddr );
        }

        /// <summary>Adds or replaces an interop callback for a global symbol</summary>
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
        public void AddInteropCallback( string symbolName, Delegate @delegate )
        {
            LLVMOrcGetMangledSymbol( JitStackHandle, out string mangledName, symbolName );
            if( GlobalInteropFunctions.TryGetValue( mangledName, out WrappedNativeCallback existingCallback ) )
            {
                GlobalInteropFunctions.Remove( mangledName );
                existingCallback.Dispose( );
            }

            GlobalInteropFunctions.Add( mangledName, new WrappedNativeCallback( @delegate ) );
        }

#if LLVM_COFF_EXPORT_BUG_FIXED
/* see: https://reviews.llvm.org/rL258665 */
        /// <inheritdoc/>
        public IJitModuleHandle LazyAddModule( BitcodeModule module, SymbolResolver resolver )
        {
            module.MakeShared( );
            var wrappedResolver = new WrappedNativeCallback( resolver );

            var err = LLVMOrcAddLazilyCompiledIR( JitStackHandle, out UInt64 retHandle, module.SharedModuleRef, wrappedResolver.NativeFuncPtr, IntPtr.Zero );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            // keep resolver delegate alive as native code needs to call it after this function exits
            SymbolResolvers.Add( retHandle, wrappedResolver );
            return ( JitModuleHandle<UInt64> )retHandle;
        }
#endif

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
                        return default;
                    }

                    AddModule( module );
                    var err = LLVMOrcGetSymbolAddress( JitStackHandle, out UInt64 implAddr, implName );
                    if( err != null )
                    {
                        throw new LlvmException( err.ToString() );
                    }

                    err = LLVMOrcSetIndirectStubPointer( JitStackHandle, mangledName, implAddr );
                    if( err != null )
                    {
                        throw new LlvmException( err.ToString() );
                    }

                    LazyFunctionGenerators.Remove( mangledName );
                    return implAddr;
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch
                {
                    return default;
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }

            var callbackAction = new WrappedNativeCallback( ( LazyFunctionGeneratorCallback )CompileAction );
            LazyFunctionGenerators.Add( mangledName, callbackAction );

            var e = LLVMOrcCreateLazyCompileCallback( JitStackHandle, out UInt64 stubAddr, callbackAction.NativeFuncPtr, context );
            if( e != null )
            {
                throw new LlvmException( e.ToString() );
            }

            e = LLVMOrcCreateIndirectStub( JitStackHandle, mangledName, stubAddr );
            if( e != null )
            {
                throw new LlvmException( e.ToString( ) );
            }
        }

        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            DisposeCallbacks( GlobalInteropFunctions );
            DisposeCallbacks( SymbolResolvers );
            DisposeCallbacks( LazyFunctionGenerators );
            JitStackHandle.Dispose( );
        }

        private static void DisposeCallbacks<T>( IDictionary<T, WrappedNativeCallback> map )
        {
            foreach( var callBack in map.Values )
            {
                callBack.Dispose( );
            }

            map.Clear( );
        }

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        private delegate UInt64 LazyFunctionGeneratorCallback( LLVMOrcJITStackRef jitStack, IntPtr ctx );

        private readonly Dictionary<string, WrappedNativeCallback> GlobalInteropFunctions = new Dictionary<string, WrappedNativeCallback>();

        private readonly Dictionary<UInt64, WrappedNativeCallback> SymbolResolvers = new Dictionary<UInt64, WrappedNativeCallback>();

        private readonly Dictionary<string, WrappedNativeCallback> LazyFunctionGenerators = new Dictionary<string, WrappedNativeCallback>();

        private readonly LLVMOrcJITStackRef JitStackHandle;
    }
}
