// <copyright file="OrcJit.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

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

        /// <inheritdoc/>
        public override bool IsDisposed => JitStackHandle == default;

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
            return (JitModuleHandle<LLVMOrcModuleHandle> )AddModule( module, DefaultSymbolResolver );
        }

        /// <summary>Add a module to the engine</summary>
        /// <param name="module">The module to add to the engine</param>
        /// <param name="resolver">Symbol resolver delegate</param>
        /// <returns>Handle for the module in the engine</returns>
        /// <remarks>
        /// <note type="note">
        /// With <see cref="OrcJit"/> the module is shared with the engine using a reference
        /// count. In this case the module is not disposed and the <see cref="BitcodeModule.IsShared"/>
        /// property is set to <see langword="true"/>. Callers may continue to use the module in this case,
        /// though modifying it or interned data from it's context may result in undefined behavior.
        /// </note>
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
            var err = LLVMOrcAddEagerlyCompiledIR( JitStackHandle, out LLVMOrcModuleHandle retHandle, module.SharedModuleRef, wrappedResolver.NativeFuncPtr, IntPtr.Zero );
#else
            // symbols are resolved if lazy compiled, requesting the address looks up the symbol in the IR module
            // where the COFF bug doesn't get in the way. The function is then JIT compiled to produce a native
            // function and the address of that function is returned.
            var err = LLVMOrcAddLazilyCompiledIR( JitStackHandle, out LLVMOrcModuleHandle retHandle, module.SharedModuleRef, wrappedResolver.NativeFuncPtr, IntPtr.Zero );
#endif
            if(err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            // keep resolver delegate alive as native code needs to call it after this function exits
            SymbolResolvers.Add( retHandle, wrappedResolver );
            return ( JitModuleHandle<LLVMOrcModuleHandle> )retHandle;
        }

        /// <inheritdoc/>
        public void RemoveModule( IJitModuleHandle handle )
        {
            if( !( handle is JitModuleHandle<LLVMOrcModuleHandle> orcHandle ) )
            {
                throw new ArgumentException( "Invalid handle provided", nameof( handle ) );
            }

            var err = LLVMOrcRemoveModule( JitStackHandle, orcHandle );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            SymbolResolvers.Remove( orcHandle );
        }

        /// <inheritdoc/>
        public UInt64 DefaultSymbolResolver( string name, IntPtr ctx )
        {
            try
            {
                var err = LLVMOrcGetSymbolAddress( JitStackHandle, out LLVMOrcTargetAddress retAddr, name );
                if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
                {
                    throw new InvalidOperationException($"Unresolved Symbol: '{name}'; {LLVMOrcGetErrorMsg( JitStackHandle )}");
                }

                if( retAddr.Address != 0 )
                {
                    return retAddr.Address;
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
            var err = LLVMOrcGetSymbolAddress( JitStackHandle, out LLVMOrcTargetAddress retAddr, name );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            if( retAddr.Address == 0 )
            {
                throw new KeyNotFoundException( $"Function {name} not found" );
            }

            return Marshal.GetDelegateForFunctionPointer<T>( ( IntPtr )retAddr.Address );
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
            if(GlobalInteropFunctions.TryGetValue( mangledName, out WrappedNativeCallback existingCallback ))
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

            var err = LLVMOrcAddLazilyCompiledIR( JitStackHandle, out LLVMOrcModuleHandle retHandle, module.SharedModuleRef, wrappedResolver.NativeFuncPtr, IntPtr.Zero );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            // keep resolver delegate alive as native code needs to call it after this function exits
            SymbolResolvers.Add( retHandle, wrappedResolver );
            return ( JitModuleHandle<LLVMOrcModuleHandle> )retHandle;
        }
#endif

        /// <inheritdoc/>
        public void AddLazyFunctionGenerator( string name, LazyFunctionCompiler generator, IntPtr context )
        {
            LLVMOrcGetMangledSymbol( JitStackHandle, out string mangledName, name );

            // wrap the provided generator function for a safe native callback
            LazyFunctionGeneratorCallback compileAction = ( IntPtr ctx ) =>
            {
                try
                {
                    ( string implName, BitcodeModule module) = generator( );
                    if( module == null )
                    {
                        return default;
                    }

                    AddModule( module );
                    var err = LLVMOrcGetSymbolAddress( JitStackHandle, out LLVMOrcTargetAddress implAddr, implName );
                    if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
                    {
                        throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
                    }

                    err = LLVMOrcSetIndirectStubPointer( JitStackHandle, mangledName, implAddr );
                    if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
                    {
                        throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
                    }

                    LazyFunctionGenerators.Remove( mangledName );
                    return implAddr;
                }
                catch
                {
                    return default;
                }
            };

            var callbackAction = new WrappedNativeCallback( compileAction );
            LazyFunctionGenerators.Add( mangledName, callbackAction );

            var e = LLVMOrcCreateLazyCompileCallback( JitStackHandle, out LLVMOrcTargetAddress stubAddr, callbackAction.NativeFuncPtr, context );
            if( e != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            e = LLVMOrcCreateIndirectStub( JitStackHandle, mangledName, stubAddr );
            if( e != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }
        }

        /// <inheritdoc/>
        protected override void InternalDispose( bool disposing )
        {
            var err = LLVMOrcDisposeInstance( JitStackHandle );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            DisposeCallbacks( GlobalInteropFunctions );
            DisposeCallbacks( SymbolResolvers );
            DisposeCallbacks( LazyFunctionGenerators );
        }

        private static void DisposeCallbacks<T>(IDictionary<T,WrappedNativeCallback> map)
        {
            foreach( var callBack in map.Values)
            {
                callBack.Dispose( );
            }

            map.Clear( );
        }

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        private delegate LLVMOrcTargetAddress LazyFunctionGeneratorCallback( IntPtr ctx );

        private Dictionary<string, WrappedNativeCallback> GlobalInteropFunctions = new Dictionary<string, WrappedNativeCallback>();

        private Dictionary<LLVMOrcModuleHandle, WrappedNativeCallback> SymbolResolvers = new Dictionary<LLVMOrcModuleHandle, WrappedNativeCallback>();

        private Dictionary<string, WrappedNativeCallback> LazyFunctionGenerators = new Dictionary<string, WrappedNativeCallback>();

        private readonly LLVMOrcJITStackRef JitStackHandle;

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMSharedObjectBufferRef LLVMOrcMakeSharedObjectBuffer( LLVMMemoryBufferRef ObjBuffer );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMOrcDisposeSharedObjectBufferRef( IntPtr SharedObjBuffer );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcJITStackRef LLVMOrcCreateInstance( LLVMTargetMachineRef @TM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false)]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        private static extern string LLVMOrcGetErrorMsg( LLVMOrcJITStackRef @JITStack );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern void LLVMOrcGetMangledSymbol( LLVMOrcJITStackRef @JITStack
                                                          , [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "MangledSymbol" )] out string @MangledSymbol
                                                          , [MarshalAs( UnmanagedType.LPStr )] string @Symbol
                                                          );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcCreateLazyCompileCallback( LLVMOrcJITStackRef @JITStack, out LLVMOrcTargetAddress retAddr, IntPtr @Callback, IntPtr @CallbackCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMOrcErrorCode LLVMOrcCreateIndirectStub( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @InitAddr );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMOrcErrorCode LLVMOrcSetIndirectStubPointer( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @NewAddr );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcAddEagerlyCompiledIR( LLVMOrcJITStackRef @JITStack, out LLVMOrcModuleHandle retHandle, LLVMSharedModuleRef @Mod, IntPtr @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcAddLazilyCompiledIR( LLVMOrcJITStackRef @JITStack, out LLVMOrcModuleHandle retHandle, LLVMSharedModuleRef @Mod, IntPtr @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcAddObjectFile( LLVMOrcJITStackRef @JITStack, out LLVMOrcModuleHandle retHandle, LLVMSharedObjectBufferRef @Obj, IntPtr @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcRemoveModule( LLVMOrcJITStackRef @JITStack, LLVMOrcModuleHandle @H );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMOrcErrorCode LLVMOrcGetSymbolAddress( LLVMOrcJITStackRef @JITStack, out LLVMOrcTargetAddress retAddr, [MarshalAs( UnmanagedType.LPStr )] string @SymbolName );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcDisposeInstance( LLVMOrcJITStackRef @JITStack );
    }
}
