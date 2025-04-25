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

using System.Collections.Immutable;

// Apply using aliases to simplify avoidance of name conflicts.
using InteropCodeGenTarget = Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.LibLLVMCodeGenTarget;
using InteropItf = Ubiquity.NET.Llvm.Interop.ILibLlvm;
using InteropLib = Ubiquity.NET.Llvm.Interop.Library;
using InteropTargetRegistration = Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.LibLLVMTargetRegistrationKind;

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
        public ImmutableArray<CodeGenTarget> SupportedTargets => [ .. ItfImpl.SupportedTargets.Cast<CodeGenTarget>() ];

        /// <inheritdoc/>
        public void RegisterTarget(CodeGenTarget target, TargetRegistration registrations = TargetRegistration.All)
        {
            ItfImpl.RegisterTarget((InteropCodeGenTarget)target, (InteropTargetRegistration)registrations);
        }

        /// <summary>Initializes the native LLVM library support</summary>
        /// <returns><see cref="ILibLlvm"/> implementation for the library</returns>
        [MustUseReturnValue]
        public static ILibLlvm InitializeLLVM()
        {
            return new Library(InteropLib.InitializeLLVM());
        }

        /// <summary>Gets the native target for the current runtime</summary>
        public static CodeGenTarget NativeTarget => (CodeGenTarget)RuntimeInformation.ProcessArchitecture.GetLibLLVMTarget();

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
