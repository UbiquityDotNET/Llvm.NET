// -----------------------------------------------------------------------
// <copyright file="IExecutionEngine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.JIT
{
    /// <summary>Common interface for an Execution engine</summary>
    public interface IExecutionEngine
    {
        /// <summary>Gets the Target machine for this engine</summary>
        TargetMachine TargetMachine { get; }

        /// <summary>Add a module to the engine</summary>
        /// <param name="bitcodeModule">The module to add to the engine</param>
        /// <param name="resolver">Symbol resolver delegate</param>
        /// <returns>Handle for the module in the engine</returns>
        /// <remarks>
        /// <note type="warning">
        /// Ownership of the <paramref name="bitcodeModule"/> is transferred to the JIT engine and therefore,
        /// after successful completion of this call the module reports as disposed.
        /// </note>
        /// <note type="important">
        /// The <paramref name="resolver"/> must not throw an exception as the native LLVM JIT engine
        /// won't understand it and would leave the engine and LLVM in an inconsistent state. If the
        /// symbol isn't found LLVM generates an error message in debug builds and in all builds, terminates
        /// the application.
        /// </note>
        /// </remarks>
        ulong AddEagerlyCompiledModule( BitcodeModule bitcodeModule, LLVMOrcSymbolResolverFn resolver );

        /// <summary>Removes a module from the engine</summary>
        /// <param name="handle">handle to remove</param>
        /// <remarks>
        /// This effectively transfers ownership of the module back to the caller.
        /// </remarks>
        void RemoveModule( ulong handle );

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
        T GetFunctionDelegate<T>( string name );

        /// <summary>Implementation of a default symbol resolver</summary>
        /// <param name="name">Symbol name to resolve</param>
        /// <param name="ctx">Resolver context</param>
        /// <returns>Address of the symbol</returns>
        ulong DefaultSymbolResolver( string name, IntPtr ctx );
    }

    /// <summary>Delegate for a lazy JIT function generator</summary>
    /// <returns>Name of the function implementation and the <see cref="BitcodeModule"/> it was generated into</returns>
    public delegate (string, BitcodeModule) LazyFunctionCompiler( /*IntPtr context*/ );

    /// <summary>Interface for lazy compilation JIT support</summary>
    public interface ILazyCompileExecutionEngine
        : IExecutionEngine
    {
        /// <summary>Adds a module for lazy compilation</summary>
        /// <param name="bitcodeModule">The module to add to the engine</param>
        /// <param name="resolver">Symbol resolver delegate</param>
        /// <returns>Handle for the module in the engine</returns>
        /// <remarks>
        /// <note type="warning">
        /// <para>For the JIT engine the input <paramref name="bitcodeModule"/> is disconnected from
        /// the underlying LLVM module as the module is considered fully owned by the engine.</para>
        /// </note>
        /// </remarks>
        ulong AddLazyCompiledModule( BitcodeModule bitcodeModule, LLVMOrcSymbolResolverFn resolver );

// LLVM 10 is transitioning to ORC JIT v2 and unfortunately,
// the lazy function generator is doing it's own symbol resolution that doesn't
// account for the COFF export bug
#if LAZY_FUNCTION_GENERATOR_SUPPORTED
        /// <summary>Add a lazy function generator</summary>
        /// <param name="name">name of the function</param>
        /// <param name="generator">Generator that will generate the bit-code module for the function on demand</param>
        /// <param name="context">Opaque context to pass to <paramref name="generator"/> when the function is first called</param>
        /// <remarks>
        /// A lazy function generator is an engine callback that is called when the JIT engine first encounters
        /// a symbol name during execution. If the function is never called, then the code is never generated (e.g.
        /// this is the real "JIT" part of a JIT engine.)
        /// <para>The <paramref name="generator"/> generates the IR module for the function. Typically, the generator
        /// is a closure that captured the AST for the language and generates the IR from that state. However, the
        /// context is available as a means to identify the function to generate.</para>
        /// </remarks>
        void AddLazyFunctionGenerator( string name, LazyFunctionCompiler generator, IntPtr context );
#endif
    }

    /// <summary>Extension class to add common default behavior for implementations of ILazyCompileExecutionEngine</summary>
    /// <remarks>
    /// Once C# supports default interface methods, these can move directly to the interface definition
    /// </remarks>
    public static class LazyCompilationExecutionEngineExtensions
    {
#if LAZY_FUNCTION_GENERATOR_SUPPORTED
        /// <summary>Add a lazy function generator</summary>
        /// <param name="jit">JIT to add the module to</param>
        /// <param name="name">name of the function</param>
        /// <param name="generator">Generator that will generate the bit-code module for the function on demand</param>
        /// <remarks>
        /// A lazy function generator is an engine callback that is called when the JIT engine first encounters
        /// a symbol name during execution. If the function is never called, then the code is never generated (e.g.
        /// this is the real "JIT" part of a JIT engine.)
        /// <para>The <paramref name="generator"/> generates the IR module for the function. For this overload, the generator
        /// must contain everything necessary to generate the <see cref="BitcodeModule"/> for the function. Typically, this is
        /// a closure that captured the AST for the language and generates the IR from that state.</para>
        /// </remarks>
        public static void AddLazyFunctionGenerator( [ValidatedNotNull] this ILazyCompileExecutionEngine jit, string name, LazyFunctionCompiler generator )
        {
            jit.ValidateNotNull( nameof( jit ) );
            jit.AddLazyFunctionGenerator( name, generator, IntPtr.Zero );
        }
#endif

        /// <summary>Adds a module to the JIT for lazy compilation using the engine's default symbol resolver</summary>
        /// <param name="jit">JIT engine to add the module to</param>
        /// <param name="module">module to add</param>
        /// <returns>Handle for the module in the engine</returns>
        public static ulong AddEagerlyCompiledModule( [ValidatedNotNull] this IExecutionEngine jit, BitcodeModule module )
        {
            jit.ValidateNotNull( nameof( jit ) );
            return jit.AddEagerlyCompiledModule( module, jit.DefaultSymbolResolver );
        }

        /// <summary>Adds a module to the JIT for lazy compilation using the engine's default symbol resolver</summary>
        /// <param name="jit">JIT engine to add the module to</param>
        /// <param name="module">module to add</param>
        /// <returns>Handle for the module in the engine</returns>
        public static ulong AddLazyCompiledModule( [ValidatedNotNull] this ILazyCompileExecutionEngine jit, BitcodeModule module )
        {
            jit.ValidateNotNull( nameof( jit ) );
            return jit.AddLazyCompiledModule( module, jit.DefaultSymbolResolver );
        }
    }
}
