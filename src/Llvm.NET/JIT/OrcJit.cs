// <copyright file="OrcJit.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
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
    public sealed class OrcJit
        : DisposableObject
        , IExecutionEngine
    {
        /// <summary>Initializes a new instance of the <see cref="OrcJit"/> class for a given target machine.</summary>
        /// <param name="machine">Target machine for the JIT</param>
        public OrcJit( TargetMachine machine )
        {
            JitStackHandle = LLVMOrcCreateInstance( machine.TargetMachineHandle );
        }

        /// <inheritdoc/>
        public TargetMachine TargetMachine { get; }

        /// <inheritdoc/>
        public override bool IsDisposed => JitStackHandle == default;

        /// <summary>JIT call back for symbol resolution</summary>
        /// <param name="name">Name of the symbol</param>
        /// <param name="lookupCtx">unused</param>
        /// <returns>Address of the symbol</returns>
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate ulong SymbolResolver( [MarshalAs( UnmanagedType.LPStr )] string name, IntPtr lookupCtx );

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
        /// The <paramref name="resolver"/> must not throw an exception as the native LLVM jit engine
        /// won't understand it and would leave the engine and LLVM in an inconsistent state. If the
        /// symbol isn't found LLVM generates an error message in debug builds and in all builds, terminates
        /// the application.
        /// </note>
        /// </remarks>
        public IJitModuleHandle AddModule( BitcodeModule module, SymbolResolver resolver )
        {
            module.MakeShared( );
            var wrappedResolver = new WrappedNativeCallback( resolver );

            var err = LLVMOrcAddLazilyCompiledIR( JitStackHandle, out LLVMOrcModuleHandle retHandle, module.SharedModuleRef, wrappedResolver.NativeFuncPtr, IntPtr.Zero );
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

        /// <summary>Implementation of a default symbol resolver</summary>
        /// <param name="name">Symbol name to resolve</param>
        /// <param name="ctx">Resolver context</param>
        /// <returns>Address of the symbol</returns>
        public UInt64 DefaultSymbolResolver( string name, IntPtr ctx )
        {
            var err = LLVMOrcGetSymbolAddress( JitStackHandle, out LLVMOrcTargetAddress retAddr, name );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            if( retAddr.Address != 0 )
            {
                return retAddr.Address;
            }

            if( GlobalInteropFunctions.TryGetValue( name, out WrappedNativeCallback callBack) )
            {
                return ( UInt64 )callBack.NativeFuncPtr.ToInt64( );
            }

            return 0;
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
                return default;
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
        /// Dispose method was called on that instance, then the result will be the callback
        /// operates on a disposed object)
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

        /// <inheritdoc/>
        protected override void InternalDispose( bool disposing )
        {
            var err = LLVMOrcDisposeInstance( JitStackHandle );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            foreach( var callBack in GlobalInteropFunctions.Values )
            {
                callBack.Dispose( );
            }

            GlobalInteropFunctions.Clear( );
        }

        private Dictionary<string, WrappedNativeCallback> GlobalInteropFunctions = new Dictionary<string, WrappedNativeCallback>();

        private Dictionary<LLVMOrcModuleHandle, WrappedNativeCallback> SymbolResolvers = new Dictionary<LLVMOrcModuleHandle, WrappedNativeCallback>();

        private LLVMOrcJITStackRef JitStackHandle;

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
        private static extern LLVMOrcErrorCode LLVMOrcCreateLazyCompileCallback( LLVMOrcJITStackRef @JITStack, out LLVMOrcTargetAddress retAddr, LLVMOrcLazyCompileCallbackFn @Callback, IntPtr @CallbackCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMOrcErrorCode LLVMOrcCreateIndirectStub( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @InitAddr );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMOrcErrorCode LLVMOrcSetIndirectStubPointer( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @NewAddr );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcAddEagerlyCompiledIR( LLVMOrcJITStackRef @JITStack, out LLVMOrcModuleHandle retHandle, LLVMSharedModuleRef @Mod, SymbolResolver @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcAddLazilyCompiledIR( LLVMOrcJITStackRef @JITStack, out LLVMOrcModuleHandle retHandle, LLVMSharedModuleRef @Mod, IntPtr @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcAddObjectFile( LLVMOrcJITStackRef @JITStack, out LLVMOrcModuleHandle retHandle, LLVMSharedObjectBufferRef @Obj, SymbolResolver @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcRemoveModule( LLVMOrcJITStackRef @JITStack, LLVMOrcModuleHandle @H );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMOrcErrorCode LLVMOrcGetSymbolAddress( LLVMOrcJITStackRef @JITStack, out LLVMOrcTargetAddress retAddr, [MarshalAs( UnmanagedType.LPStr )] string @SymbolName );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcDisposeInstance( LLVMOrcJITStackRef @JITStack );
    }
}
