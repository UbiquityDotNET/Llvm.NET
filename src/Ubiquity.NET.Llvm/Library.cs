// -----------------------------------------------------------------------
// <copyright file="Library.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

/*
This is mostly a simple wrapper around the interop code. It exists here to aid in isolating consumers
of this library from direct dependencies on the interop library. It is marked as private assets = "all"
so that it does not automatically flow as a dependency to consumers. If a consumer has a reason to access
the low level interop (Test code sometimes does) it must explicitly reference it.
*/

using InteropCodeGenTarget = Ubiquity.NET.Llvm.Interop.CodeGenTarget;
using InteropItf = Ubiquity.NET.Llvm.Interop.ILibLlvm;
using InteropLib = Ubiquity.NET.Llvm.Interop.Library;
using InteropTargetRegistration = Ubiquity.NET.Llvm.Interop.TargetRegistration;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public sealed class Library
        : ILibLlvm
    {
        /// <inheritdoc/>
        public void Dispose() => ItfImpl.Dispose();

        /// <inheritdoc/>
        public void RegisterTarget(CodeGenTarget target, TargetRegistration registrations = TargetRegistration.All)
            => ItfImpl.RegisterTarget((InteropCodeGenTarget)target, (InteropTargetRegistration)registrations);

        // TODO: Does LLVM 20 fix the problem of re-init from same process? [Some static init wasn't re-run and stale data left in place]

        /// <summary>Initializes the native LLVM library support</summary>
        /// <returns>
        /// <see cref="ILibLlvm"/> implementation for the library
        /// </returns>
        /// <remarks>
        /// This can only be called once per application to initialize the
        /// LLVM library. <see cref="System.IDisposable.Dispose()"/> will release
        /// any resources allocated by the library. The V10 LLVM library does
        /// *NOT* support re-initialization within the same process. Thus, this
        /// is best used at the top level of the application and released at or
        /// near process exit.
        /// </remarks>
        public static ILibLlvm InitializeLLVM()
        {
            return new Library(InteropLib.InitializeLLVM());
        }

        internal Library(InteropItf impl)
        {
            ItfImpl = impl;
        }

        private readonly InteropItf ItfImpl;
    }
}
