// <copyright file="Library.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.TargetRegistration;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.ErrorHandling;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public sealed class Library
        : ILibLlvm
    {
        /// <inheritdoc/>
        public ImmutableArray<LibLLVMCodeGenTarget> SupportedTargets => LazyTargets.Value;

        /// <inheritdoc/>
        public void RegisterTarget( LibLLVMCodeGenTarget target, LibLLVMTargetRegistrationKind registrations = LibLLVMTargetRegistrationKind.TargetRegistration_All )
        {
            // NOTE: All logic for registering the targets is in native code.
            LibLLVMRegisterTarget( target, registrations ).ThrowIfFailed();
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            LLVMResetFatalErrorHandler();
            LLVMShutdown();
        }

        /// <summary>Initializes the native LLVM library support</summary>
        /// <returns><see cref="ILibLlvm"/> implementation for the library</returns>
        public static ILibLlvm InitializeLLVM()
        {
            // If this is the first call and the library resolver is applied, then validate
            // the version from the library for sanity.
            if(!NativeLibraryResolver.Apply())
            {
                throw new InvalidOperationException("LLVM library was previously initialized. Re-init is not supported in the native library");
            }

            // Verify the version of LLVM in LibLLVM.
            LLVMGetVersion( out uint actualMajor, out uint actualMinor, out uint actualPatch );
            if(actualMajor != SupportedVersionMajor
            || actualMinor != SupportedVersionMinor
            || actualPatch < SupportedVersionPatch // allow later patches...
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

        private Library()
        {
            unsafe
            {
                LLVMInstallFatalErrorHandler( &FatalErrorHandler );
            }
        }

        private readonly Lazy<ImmutableArray<LibLLVMCodeGenTarget>> LazyTargets = new(GetSupportedTargets);

        // Expected version info for verification of matched LibLLVM
        private const int SupportedVersionMajor = 20;
        private const int SupportedVersionMinor = 1;
        private const int SupportedVersionPatch = 3;

        // Singleton initializer for the supported targets array
        private static ImmutableArray<LibLLVMCodeGenTarget> GetSupportedTargets( )
        {
            var resultArray = new LibLLVMCodeGenTarget[LibLLVMGetNumTargets()];
            LibLLVMGetRuntimeTargets( resultArray, resultArray.Length ).ThrowIfFailed();
            // Create a new immutable array without copy (Wraps the input array)
            return ImmutableCollectionsMarshal.AsImmutableArray( resultArray );
        }

        // Native call back for fatal error handling.
        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe void FatalErrorHandler( byte* reason )
        {
            try
            {
                // NOTE: LLVM will call exit() upon return from this function and there's no way to stop it
                Trace.TraceError( "LLVM Fatal Error: '{0}'; Application will exit.", ExecutionEncodingStringMarshaller.ConvertToManaged( reason ) );
            }
            catch(Exception ex)
            {
                // No finalizers will occur after this, it's a HARD termination of the app.
                // LLVM will do that on return but this can at least indicate a different problem
                // from the original LLVM was reporting.
                Environment.FailFast( $"Unhandled exception in {nameof( FatalErrorHandler )}.", ex );
            }
        }
    }
}
