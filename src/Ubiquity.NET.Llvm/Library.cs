// -----------------------------------------------------------------------
// <copyright file="Library.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

/*
This is mostly a simple wrapper around the interop code. It exists here to aid in isolating consumers
of this library from direct dependencies on the interop library. If a consumer has a reason to access
the low level interop (Test code sometimes does) it must explicitly reference it.
*/

using Ubiquity.NET.Extensions;

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
        public void Dispose()
        {
            ItfImpl.Dispose();
        }

        /// <inheritdoc/>
        public void RegisterTarget(CodeGenTarget target, TargetRegistration registrations = TargetRegistration.All)
        {
            ItfImpl.RegisterTarget((InteropCodeGenTarget)target, (InteropTargetRegistration)registrations);
        }

        /// <summary>Initializes the native LLVM library support</summary>
        /// <param name="target">Target to use in resolving the proper library that implements the LLVM native code. [Default: CodeGenTarget.Native]</param>
        /// <returns><see cref="ILibLlvm"/> implementation for the library</returns>
        /// <remarks>
        /// <para>This can be called multiple times per application BUT all such calls MUST use the same value for
        /// <paramref name="target"/> in order to load the underlying native LLVM library.</para>
        /// <para><see cref="Dispose()"/> will release any resources allocated by the library but NOT the library itself.
        /// That is loaded once the first time this is called. The .NET runtime does *NOT* support re-load of a P/Invoke
        /// library within the same process. Thus, this is best used at the top level of the application and released at
        /// or near process exit. An access violation crash is likely to occur if any attempts to use the library's functions
        /// occurs after it is unloaded as there is no way to invalidate the results of resolving the method + library into
        /// an address.</para>
        /// <para>While any variant of the native library will support <see cref="CodeGenTarget.Native"/> they can support up
        /// to one other target. Thus if the consumer is ever going to support cross-platform scenarios, then it MUST specify
        /// the target the first time this is called. This restriction is a tradeoff from the cost of building the native interop
        /// library. Building all possible processor targets into a single library for every possible runtime is just not feasible
        /// in the automated builds for most projects let alone a no budget OSS project like this one.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">Native Interop library already loaded for a different target</exception>
        /// <exception cref="ArgumentOutOfRangeException">The target provided is undefined or <see cref="CodeGenTarget.All"/></exception>
        [MustUseReturnValue]
        public static ILibLlvm InitializeLLVM(CodeGenTarget target = CodeGenTarget.Native)
        {
            return new Library(InteropLib.InitializeLLVM((InteropCodeGenTarget)target));
        }

        // "MOVE" construction, this instance takes over responsibility
        // of calling dispose.
        internal Library(InteropItf impl)
        {
            ItfImpl = impl;
        }

        [SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP008:Don't assign member with injected and created disposables", Justification = "MOVE semantics mean owned by this instance now" )]
        private readonly InteropItf ItfImpl;
    }
}
