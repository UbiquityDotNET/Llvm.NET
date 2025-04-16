// <copyright file="Library.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.TargetRegistration;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public sealed class Library
        : ILibLlvm
    {
        /// <inheritdoc/>
        public ImmutableArray<LibLLVMCodeGenTarget> Targets => LazyTargets.Value;

        /// <inheritdoc/>
        public void RegisterTarget( LibLLVMCodeGenTarget target, LibLLVMTargetRegistrationKind registrations = LibLLVMTargetRegistrationKind.TargetRegistration_All )
        {
            // NOTE: All logic for registering the targets is in native code.
            //       If an invalid target is provided for the library loaded
            //       in InitializeLLVM() below then the native code generates
            //       an error which is transformed to an exception here.
            //       Native and All are always allowed as they are generic
            //       terms that are understood by the native code.
            LibLLVMRegisterTarget( target, registrations ).ThrowIfFailed();
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            LLVMShutdown();
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
        /// occurs before it is loaded or after it is unloaded as there is no way to invalidate the results of resolving the
        /// method + library into an address.</para>
        /// <para>While any variant of the native library will support <see cref="LibLLVMCodeGenTarget.CodeGenTarget_Native"/>
        /// they can support up to one other target. Thus if the consumer is ever going to support/ cross-platform scenarios,
        /// then it MUST specify the target the first time this is called. This restriction is a tradeoff from the cost of
        /// building the native interop library. Building all possible processor targets into a single library for every possible
        /// runtime is just not feasible in the automated builds for most projects let alone a no budget OSS project like this
        /// one.</para>
        /// </remarks>
        /// <ImplementationNote>
        /// The constraint on native+one target is a limitation of the build requirements of a Free (as in beer) OSS project. It is
        /// still theoretical at best. (Needs  testing of native library generation in automated builds) Currently this library ONLY
        /// supports Win-x64 as the native target/runtime BUT any target supported by LLVM is theoretically OK. (But for future compat
        /// the restriction on initialization is retained, callers cannot re-init to a different target)
        /// </ImplementationNote>
        /// <exception cref="InvalidOperationException">Native Interop library already loaded for a different target</exception>
        /// <exception cref="ArgumentOutOfRangeException">The target provided is undefined or <see cref="LibLLVMCodeGenTarget.CodeGenTarget_All"/></exception>
        public static ILibLlvm InitializeLLVM( LibLLVMCodeGenTarget target )
        {
            NativeLibraryResolver.Apply(target);

            // Verify the version of LLVM in LibLLVM, this will trigger the resolver to load
            // the DLL and set the Native Library handle in NativeLibHandle if not already resolved
            LLVMGetVersion( out uint actualMajor, out uint actualMinor, out uint actualPatch );
            if(actualMajor != SupportedVersionMajor
            || actualMinor != SupportedVersionMinor
            || actualPatch < SupportedVersionPatch
            )
            {
                string msgFmt = Resources.Mismatched_LibLLVM_version_Expected_0_1_2_Actual_3_4_5;
                string msg = string.Format( CultureInfo.CurrentCulture
                                          , msgFmt
                                          , SupportedVersionMajor
                                          , SupportedVersionMinor
                                          , SupportedVersionPatch
                                          , actualMajor
                                          , actualMinor
                                          , actualPatch
                                          );
                throw new InvalidOperationException( msg );
            }

            return new Library();
        }

        private readonly Lazy<ImmutableArray<LibLLVMCodeGenTarget>> LazyTargets = new(GetSupportedTargets);

        // Expected version info for verification of matched LibLLVM
        private const int SupportedVersionMajor = 20;
        private const int SupportedVersionMinor = 1;
        private const int SupportedVersionPatch = 0;

        // Singleton initializer for the supported targets array
        private static ImmutableArray<LibLLVMCodeGenTarget> GetSupportedTargets( )
        {
            var resultArray = new LibLLVMCodeGenTarget[LibLLVMGetNumTargets()];
            LibLLVMGetRuntimeTargets( resultArray, resultArray.Length ).ThrowIfFailed();
            // Create a new immutable array without copy (Wraps the input array)
            return ImmutableCollectionsMarshal.AsImmutableArray( resultArray );
        }
    }
}
