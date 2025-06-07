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

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AttributeBindings;

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
        public void Dispose( )
        {
            ItfImpl.Dispose();
        }

        /// <inheritdoc/>
        public ImmutableArray<CodeGenTarget> SupportedTargets => [ .. ItfImpl.SupportedTargets.Cast<CodeGenTarget>() ];

        /// <inheritdoc/>
        public ImmutableDictionary<LazyEncodedString, AttributeInfo> AttributeMap => LazyAttributeMap.Value;

        /// <inheritdoc/>
        public void RegisterTarget( CodeGenTarget target, TargetRegistration registrations = TargetRegistration.All )
        {
            ItfImpl.RegisterTarget( (InteropCodeGenTarget)target, (InteropTargetRegistration)registrations );
        }

        /// <summary>Initializes the native LLVM library support</summary>
        /// <returns><see cref="ILibLlvm"/> implementation for the library</returns>
        [MustUseReturnValue]
        public static ILibLlvm InitializeLLVM( )
        {
            return new Library( InteropLib.InitializeLLVM() );
        }

        /// <summary>Gets the native target for the current runtime</summary>
        public static CodeGenTarget NativeTarget => (CodeGenTarget)RuntimeInformation.ProcessArchitecture.AsLLVMTarget();

        // "MOVE" construction, this instance takes over responsibility
        // of calling dispose.
        internal Library( InteropItf impl )
        {
            ItfImpl = impl;
            LazyAttributeMap = new( CreateSingletonAttributeMap() );
        }

        [SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP008:Don't assign member with injected and created disposables", Justification = "MOVE semantics mean owned by this instance now" )]
        private readonly InteropItf ItfImpl;

        private readonly Lazy<ImmutableDictionary<LazyEncodedString, AttributeInfo>> LazyAttributeMap;

        private static ImmutableDictionary<LazyEncodedString, AttributeInfo> CreateSingletonAttributeMap( )
        {
            var attribNames = GetKnownAttributes();

            var bldr = ImmutableDictionary.CreateBuilder<LazyEncodedString, AttributeInfo>();
            foreach(LazyEncodedString name in attribNames)
            {
                bldr.Add( name, AttributeInfo.From( name ) );
            }

            return bldr.ToImmutable();
        }

        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "ppData makes sense and matches the API" )]
        private static ImmutableArray<LazyEncodedString> GetKnownAttributes( )
        {
            int len = checked((int)LibLLVMGetNumKnownAttribs());
            unsafe
            {
                byte** ppData = stackalloc byte*[len];
                using LLVMErrorRef errorRef = LibLLVMGetKnownAttributeNames(ppData, (nuint)len);
                errorRef.ThrowIfFailed();

                // Capture strings with lazy encoding
                var bldr = ImmutableArray.CreateBuilder<LazyEncodedString>(len);
                for(int i = 0; i < len; ++i)
                {
                    // Should NEVER get a null string but just in case treat it as an empty string.
                    bldr.Add( LazyEncodedString.FromUnmanaged( ppData[ i ] ) ?? LazyEncodedString.Empty );
                }

                return bldr.ToImmutable();
            }
        }
    }
}
