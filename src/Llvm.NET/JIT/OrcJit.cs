// <copyright file="OrcJit.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.JIT.OrcJit;
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
        , IExecutionEngine<OrcJitHandle>
    {
        /// <summary>Handle for a module in the OrcJit</summary>
        public struct OrcJitHandle
            : IEquatable<OrcJitHandle>
        {
            public override int GetHashCode( ) => Handle.GetHashCode( );

            public override bool Equals( object obj )
            {
                if( obj is OrcJitHandle h )
                {
                    return Equals( h );
                }

                return base.Equals( obj );
            }

            public bool Equals( OrcJitHandle other ) => Handle.Equals( other.Handle );

            public static bool operator ==( OrcJitHandle lhs, OrcJitHandle rhs ) => lhs.Equals( rhs );

            public static bool operator !=( OrcJitHandle lhs, OrcJitHandle rhs ) => !lhs.Equals( rhs );

            internal OrcJitHandle( LLVMOrcModuleHandle handle )
            {
                Handle = handle;
            }

            internal readonly LLVMOrcModuleHandle Handle;
        }

        /// <summary>Initializes a new instance of the <see cref="OrcJit"/> class for a given target machine.</summary>
        /// <param name="machine">Target machine for the JIT</param>
        public OrcJit( TargetMachine machine )
        {
            JitStackHandle = LLVMOrcCreateInstance( machine.TargetMachineHandle );
        }

        /// <inheritdoc/>
        public TargetMachine TargetMachine { get; }

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
        public OrcJitHandle AddModule( BitcodeModule module )
        {
            return AddModule( module, DefaultSymbolResolver );
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
        /// </remarks>
        public OrcJitHandle AddModule( BitcodeModule module, SymbolResolver resolver )
        {
            module.MakeShared( );
            var err = LLVMOrcAddEagerlyCompiledIR( JitStackHandle, out LLVMOrcModuleHandle retHandle, module.SharedModuleRef, resolver, IntPtr.Zero );
            if(err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            return new OrcJitHandle( retHandle );
        }

        /// <inheritdoc/>
        public void RemoveModule( OrcJitHandle handle )
        {
            var err = LLVMOrcRemoveModule( JitStackHandle, handle.Handle );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }
        }

        public UInt64 DefaultSymbolResolver( string name, IntPtr ctx )
        {
            var err = LLVMOrcGetSymbolAddress( JitStackHandle, out LLVMOrcTargetAddress retAddr, name );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            return retAddr.Address;
        }

        /// <inheritdoc/>
        public T GetFunctionDelegate<T>( string name )
        {
            var err = LLVMOrcGetSymbolAddress( JitStackHandle, out LLVMOrcTargetAddress retAddr, name );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }

            return Marshal.GetDelegateForFunctionPointer<T>( ( IntPtr )retAddr.Address );
        }

        /// <inheritdoc/>
        public override bool IsDisposed => JitStackHandle == default;

        /// <inheritdoc/>
        protected override void InternalDispose( bool disposing )
        {
            var err = LLVMOrcDisposeInstance( JitStackHandle );
            if( err != LLVMOrcErrorCode.LLVMOrcErrSuccess )
            {
                throw new Exception( LLVMOrcGetErrorMsg( JitStackHandle ) );
            }
        }

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
        private static extern void LLVMOrcGetMangledSymbol( LLVMOrcJITStackRef @JITStack, out IntPtr @MangledSymbol, [MarshalAs( UnmanagedType.LPStr )] string @Symbol );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern void LLVMOrcDisposeMangledSymbol( IntPtr @MangledSymbol );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcCreateLazyCompileCallback( LLVMOrcJITStackRef @JITStack, out LLVMOrcTargetAddress retAddr, LLVMOrcLazyCompileCallbackFn @Callback, IntPtr @CallbackCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMOrcErrorCode LLVMOrcCreateIndirectStub( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @InitAddr );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMOrcErrorCode LLVMOrcSetIndirectStubPointer( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @NewAddr );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcAddEagerlyCompiledIR( LLVMOrcJITStackRef @JITStack, out LLVMOrcModuleHandle retHandle, LLVMSharedModuleRef @Mod, SymbolResolver @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMOrcErrorCode LLVMOrcAddLazilyCompiledIR( LLVMOrcJITStackRef @JITStack, out LLVMOrcModuleHandle retHandle, LLVMSharedModuleRef @Mod, SymbolResolver @SymbolResolver, IntPtr @SymbolResolverCtx );

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
