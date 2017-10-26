// <copyright file="ExecutionEngin.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

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
    public sealed class ExecutionEngine
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="ExecutionEngine"/> class with an initial module</summary>
        /// <param name="module">Initial module to add to the engine</param>
        /// <param name="kind"><see cref="EngineKind"/> for the engine to create</param>
        /// <remarks>
        /// <note type="warning">
        /// The input <paramref name="module"/> is not useable after this call completes. The <see cref="BitcodeModule.IsDisposed"/>
        /// property will be true and any attempt to operate on the module will result in an <see cref="ObjectDisposedException"/>.
        /// The ownership of the module is effectively full transfered to the underlying LLVM Execution engine.
        /// </note>
        /// </remarks>
        public ExecutionEngine( BitcodeModule module, EngineKind kind )
            : this( module, kind, CodeGenOpt.Default )
        {
            Context = module.Context;
        }

        /// <summary>Initializes a new instance of the <see cref="ExecutionEngine"/> class with an initial module</summary>
        /// <param name="module">Initial module to add to the engine</param>
        /// <param name="kind"><see cref="EngineKind"/> for the engine to create</param>
        /// <param name="optLevel">Optimization level for the engine</param>
        /// <remarks>
        /// <note type="warning">
        /// The input <paramref name="module"/> is not useable after this call completes. The <see cref="BitcodeModule.IsDisposed"/>
        /// property will be true and any attempt to operate on the module will result in an <see cref="ObjectDisposedException"/>.
        /// The ownership of the module is effectively full transfered to the underlying LLVM Execution engine.
        /// </note>
        /// </remarks>
        public ExecutionEngine( BitcodeModule module, EngineKind kind, CodeGenOpt optLevel )
        {
            LLVMStatus status;
            string errMsg;

            module.ValidateNotNull( nameof( module ) );
            optLevel.ValidateDefined( nameof( optLevel ) );
            if( !OwnedModules.TryAdd( module.ModuleHandle, module ) )
            {
                throw new InternalCodeGeneratorException( "Failed to take ownership of module" );
            }

            switch( kind )
            {
            case EngineKind.Jit:
                status = LLVMCreateJITCompilerForModule( out Handle, module.ModuleHandle, ( uint )optLevel, out errMsg );
                break;

            case EngineKind.Interpreter:
                status = LLVMCreateInterpreterForModule( out Handle, module.ModuleHandle, out errMsg );
                break;

            case EngineKind.Either:
                status = LLVMCreateExecutionEngineForModule( out Handle, module.ModuleHandle, out errMsg );
                break;

            default:
                throw new ArgumentException( "Invalid EngineKind", nameof( kind ) );
            }

            module.Detach( );
            if( status.Failed )
            {
                throw new InternalCodeGeneratorException( errMsg );
            }
        }

        /// <summary>Gets the context that owned the first module added</summary>
        public Context Context { get; }

        public void RunStaticConstructors( ) => LLVMRunStaticConstructors( Handle );

        public void RunStaticDestructors( ) => LLVMRunStaticDestructors( Handle );

        /// <summary>Add a module to the engine</summary>
        /// <param name="module">The module to add to the engine</param>
        /// <remarks>
        /// <note type="warning">
        /// The input <paramref name="module"/> should not be disposed after this call completes.
        /// The ownership of the module is effectively full transfered to the underlying LLVM Execution
        /// engine until the engine is disposed or the module is removed from the engine via
        /// <see cref="RemoveModule(BitcodeModule)"/>.
        /// </note>
        /// </remarks>
        public void AddModule( BitcodeModule module )
        {
            if( !OwnedModules.TryAdd( module.ModuleHandle, module ) )
            {
                throw new InternalCodeGeneratorException( "Failed to take ownership of module" );
            }

            LLVMAddModule( Handle, module.ModuleHandle );
        }

        /// <summary>Removes a module from the engine</summary>
        /// <param name="module"><see cref="AddModule(BitcodeModule)"/> to remove</param>
        /// <remarks>
        /// This effectively transfers ownership of the module back to the caller.
        /// </remarks>
        public void RemoveModule( BitcodeModule module )
        {
            if( !OwnedModules.TryRemove( module.ModuleHandle, out BitcodeModule retVal ) )
            {
                throw new InternalCodeGeneratorException( "Module isn't owned by this engine" );
            }

            // Current LLVM-C API is a bit brain dead on this one. The return is hardcoded to 0
            // and the out error message is never used. Furthermore, the return from the C++
            // ExecutionEngine::removeModule() is ultimately ignored.
            if( LLVMRemoveModule( Handle, module.ModuleHandle, out LLVMModuleRef baseModule, out string errMsg ).Failed )
            {
                throw new InternalCodeGeneratorException( "Failed to remove module from engine" );
            }

            // MCJIT engine doesn't cleanup after a remove
            LLVMExecutionEngineClearGlobalMappingsFromModule( Handle, baseModule );
        }

        /// <summary>Tries to get a function from the engine, by name</summary>
        /// <param name="name">Name of the function to get</param>
        /// <param name="function">The function or <see langword="null"/> if not found</param>
        /// <returns><see langword="true"/> if the function was found</returns>
        public bool TryGetFunction( string name, out Function function )
        {
            function = null;

            if( LLVMFindFunction( Handle, name, out LLVMValueRef functionHandle ).Succeeded )
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
                var handle = LLVMGetExecutionEngineTargetData( Handle );
                if( handle.Handle.IsNull( ) )
                {
                    return null;
                }

                return DataLayout.FromHandle( handle, isDisposable: false );
            }
        }

        /// <summary>Gets the <see cref="TargetMachine"/> this engine is targeting</summary>
        public TargetMachine TargetMachine
        {
            get
            {
                LLVMTargetMachineRef handle = LLVMGetExecutionEngineTargetMachine( Handle );

                // REVIEW: Consider interning the handle mapping so multiple instances aren't created
                return new TargetMachine( handle, false );
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
            return Marshal.GetDelegateForFunctionPointer<T>( ( IntPtr )LLVMGetFunctionAddress( Handle, name ) );
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            // Disconnect the owned modules from the underlying LLVMModuleRef as the
            // native implementation of themodules owned by the engine are about to be
            // destroyed along with the ExecutionEngine.
            foreach(var module in OwnedModules.Values )
            {
                module.Detach( );
            }

            if( !Handle.Handle.IsNull( ) )
            {
                LLVMDisposeExecutionEngine( Handle );
            }

            Handle = default;
        }

        internal ExecutionEngine( LLVMExecutionEngineRef handle, Context context )
        {
            Handle = handle;
            Context = context;
        }

        internal static IHandleInterning<LLVMExecutionEngineRef, ExecutionEngine> CreateInterningFactory( )
        {
            return new HandleInterningMap<LLVMExecutionEngineRef, ExecutionEngine>( ( h, c ) => new ExecutionEngine( h, c )
                                                                                  , ( ee ) => LLVMDisposeExecutionEngine( ee.Handle )
                                                                                  );
        }

        private ConcurrentDictionary<LLVMModuleRef, BitcodeModule> OwnedModules = new ConcurrentDictionary<LLVMModuleRef, BitcodeModule>();

        private LLVMExecutionEngineRef Handle;
    }
}
