// <copyright file="ExecutionEngin.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.JIT
{
    /// <summary>Defines the kind of Execution engine to create</summary>
    [Flags]
    public enum EngineKind
    {
        /// <summary>Just-In-Time compilation to native code</summary>
        Jit = 1,

        /// <summary>LLVM IR interpreter</summary>
        Interpreter = 2,

        /// <summary>Either form of engine</summary>
        Either = Jit | Interpreter
    }

    /// <summary>An LLVM Execution Engine</summary>
    [Obsolete("Use OrcJit instead")]
    public sealed class LegacyExecutionEngine
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="LegacyExecutionEngine"/> class with an initial module</summary>
        /// <param name="kind"><see cref="EngineKind"/> for the engine to create</param>
        /// <remarks>This does not create the underlying engine, instead that is deferred until the first module is added</remarks>
        public LegacyExecutionEngine( EngineKind kind )
            : this( kind, CodeGenOpt.Default )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LegacyExecutionEngine"/> class with an initial module</summary>
        /// <param name="kind"><see cref="EngineKind"/> for the engine to create</param>
        /// <param name="optLevel">Optimization level for the engine</param>
        /// <remarks>This does not create the underlying engine, instead that is deferred until the first module is added</remarks>
        public LegacyExecutionEngine( EngineKind kind, CodeGenOpt optLevel )
        {
            Kind = kind;
            Optimization = optLevel;
            OwnedModules = new Dictionary<int, LLVMModuleRef>( );
        }

        /// <summary>Add a module to the engine</summary>
        /// <param name="module">The module to add to the engine</param>
        /// <remarks>
        /// <note type="warning">
        /// The input <paramref name="module"/> is disconnected from the underlying LLVM
        /// module as the module is considered fully owned by the engine. Thus, upon return
        /// the <see cref="BitcodeModule.IsDisposed"/> property is <see langword="true"/>
        /// </note>
        /// </remarks>
        /// <returns>Handle for the module in the engine</returns>
        public IJitModuleHandle AddModule( BitcodeModule module )
        {
            int handle = System.Threading.Interlocked.Increment( ref NextHandleValue ) - 1;
            lock( OwnedModules )
            {
                OwnedModules.Add( handle, module.ModuleHandle );
            }

            if( handle == 0 )
            {
                CreateEngine( module );
            }

            LLVMAddModule( EngineHandle, module.ModuleHandle );
            module.Detach( );
            return (JitModuleHandle<int>)handle;
        }

        /// <summary>Removes a module from the engine</summary>
        /// <param name="handle"><see cref="AddModule(BitcodeModule)"/> to remove</param>
        /// <remarks>
        /// This effectively transfers ownership of the module back to the caller.
        /// </remarks>
        public void RemoveModule( IJitModuleHandle handle )
        {
            if( !( handle is JitModuleHandle<int> jitHandle ) )
            {
                throw new ArgumentException( "Invalid handle provided", nameof( handle ) );
            }

            if( !OwnedModules.TryGetValue( jitHandle, out LLVMModuleRef module))
            {
                throw new ArgumentException( "Unknown handle value" );
            }

            // Current LLVM-C API is a bit brain dead on this one. The return is hard-coded to 0
            // and the out error message is never used. Furthermore, the return from the C++
            // ExecutionEngine::removeModule() is ultimately ignored.
            if( LLVMRemoveModule( EngineHandle, module, out LLVMModuleRef baseModule, out string errMsg ).Failed )
            {
                throw new InternalCodeGeneratorException( "Failed to remove module from engine" );
            }

            // MCJIT engine doesn't cleanup after a remove
            LLVMExecutionEngineClearGlobalMappingsFromModule( EngineHandle, baseModule );
            LLVMDisposeModule( module );
        }

        /// <summary>Tries to get a function from the engine, by name</summary>
        /// <param name="name">Name of the function to get</param>
        /// <param name="function">The function or <see langword="null"/> if not found</param>
        /// <returns><see langword="true"/> if the function was found</returns>
        public bool TryGetFunction( string name, out Function function )
        {
            function = null;

            if( LLVMFindFunction( EngineHandle, name, out LLVMValueRef functionHandle ).Succeeded )
            {
                function = ( Function )Value.FromHandle( functionHandle );
                return true;
            }

            return false;
        }

        /// <summary>Gets Layout information for the machine the engine is targeting</summary>
        public DataLayout TargetData
        {
            get
            {
                var handle = LLVMGetExecutionEngineTargetData( EngineHandle );
                if( handle == default )
                {
                    return null;
                }

                return DataLayout.FromHandle( handle );
            }
        }

        /// <summary>Gets the <see cref="TargetMachine"/> this engine is targeting</summary>
        public TargetMachine TargetMachine
        {
            get
            {
                var handle = LLVMGetExecutionEngineTargetMachine( EngineHandle );

                // REVIEW: Consider interning the handle mapping so multiple instances aren't created
                return new TargetMachine( handle );
            }
        }

        /// <summary>Gets a delegate for the native compiled function in the engine</summary>
        /// <typeparam name="T">Type of the delegate to retrieve</typeparam>
        /// <param name="name">Name of the function to get the delegate for</param>
        /// <returns>Callable delegate for the function or <see langword="null"/> if not found</returns>
        /// <remarks>
        /// The type <typeparamref name="T"/> must be a delegate that matches the signature of the actual
        /// function. The delegate should also have the <see cref="UnmanagedFunctionPointerAttribute"/> to indicate the
        /// calling convention and other marshaling behavior for the function.
        /// <note type="warning">
        /// Neither the signature nor the presence of the <see cref="UnmanagedFunctionPointerAttribute"/> is
        /// validated by this method. It is up to the caller to provide an appropriate delegate for the function
        /// defined in the engine. Incorrect delegates could lead to instabilities and application crashes.
        /// </note>
        /// </remarks>
        public T GetFunctionDelegate<T>( string name )
        {
            // REVIEW:
            // By looking up the LLVM Function by name it might be possible to determine if the provided delegate
            // matches the function. (Though various marshaling attributes, including possible custom marshaling
            // might need to be considered). This is nontrivial. Dynamically determining the delegate type as a
            // method /property on the Llvm.NET.Values.Function class isn't possible. The CLR doesn't support
            // dynamically constructing a type at runtime which would be specifiable as the type parameter T.
            return Marshal.GetDelegateForFunctionPointer<T>( ( IntPtr )LLVMGetFunctionAddress( EngineHandle, name ) );
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            if( EngineHandle != default )
            {
                LLVMDisposeExecutionEngine( EngineHandle );
            }

            OwnedModules.Clear( );
            EngineHandle = default;
        }

        internal LegacyExecutionEngine( LLVMExecutionEngineRef handle )
        {
            EngineHandle = handle;
        }

        internal class InterningFactory
            : HandleInterningMap<LLVMExecutionEngineRef, LegacyExecutionEngine>
        {
            internal InterningFactory( Context context )
                : base( context )
            {
            }

            private protected override LegacyExecutionEngine ItemFactory( LLVMExecutionEngineRef handle )
            {
                return new LegacyExecutionEngine( handle );
            }

            private protected override void DisposeItem( LegacyExecutionEngine item )
            {
                LLVMDisposeExecutionEngine( item.EngineHandle );
            }
        }

        private void CreateEngine( BitcodeModule module )
        {
            LLVMStatus status;
            string errMsg;

            switch( Kind )
            {
            case EngineKind.Jit:
                status = LLVMCreateJITCompilerForModule( out EngineHandle, module.ModuleHandle, ( uint )Optimization, out errMsg );
                break;

            case EngineKind.Interpreter:
                status = LLVMCreateInterpreterForModule( out EngineHandle, module.ModuleHandle, out errMsg );
                break;

            case EngineKind.Either:
                status = LLVMCreateExecutionEngineForModule( out EngineHandle, module.ModuleHandle, out errMsg );
                break;

            default:
                throw new ArgumentException( "Invalid EngineKind", nameof( Kind ) );
            }

            if( status.Failed )
            {
                throw new InternalCodeGeneratorException( errMsg );
            }
        }

        private readonly EngineKind Kind;
        private readonly CodeGenOpt Optimization;
        private Dictionary<int, LLVMModuleRef> OwnedModules;
        private int NextHandleValue;
        private LLVMExecutionEngineRef EngineHandle;
    }
}
