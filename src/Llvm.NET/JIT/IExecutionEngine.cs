// <copyright file="ExecutionEngin.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace Llvm.NET.JIT
{
    /// <summary>Common interface for an Execution engine</summary>
    /// <typeparam name="THandle">Type of handle used as "cookie" to identify each module added</typeparam>
    public interface IExecutionEngine<THandle>
        where THandle : struct, IEquatable<THandle>
    {
        /// <summary>Gets the Target machine for this engine</summary>
        TargetMachine TargetMachine { get; }

        /// <summary>Add a module to the engine</summary>
        /// <param name="module">The module to add to the engine</param>
        /// <returns>Handle for the module in the engine</returns>
        /// <remarks>
        /// <note type="warning">
        /// <para>For the legacy JIT engine the input <paramref name="module"/> is disconnected from
        /// the underlying LLVM module as the module is considered fully owned by the engine.
        /// Thus, upon return the <see cref="BitcodeModule.IsDisposed"/> property is <see langword="true"/></para>
        /// <para>For the OrcJit, however, the module is shared with the engine using a reference
        /// count. In this case <see cref="BitcodeModule.IsDisposed"/> property is <see langword="false"/> and the
        /// <see cref="BitcodeModule.IsShared"/> property is <see langword="true"/>. Callers may continue to use the
        /// module in this case, though modifying it or interned data from it's context may result in undefined
        /// behavior.</para>
        /// </note>
        /// </remarks>
        THandle AddModule( BitcodeModule module );

        /// <summary>Removes a module from the engine</summary>
        /// <param name="handle"><see cref="AddModule(BitcodeModule)"/> to remove</param>
        /// <remarks>
        /// This effectively transfers ownership of the module back to the caller.
        /// </remarks>
        void RemoveModule( THandle handle );

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
    }
}
