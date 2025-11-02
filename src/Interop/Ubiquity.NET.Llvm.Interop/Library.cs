// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
        public SemVer ExtendedAPIVersion { get; private set; }

        /// <inheritdoc/>
        public SemVer LlvmVersion
        {
            get
            {
                LLVMGetVersion( out UInt32 major, out UInt32 minor, out UInt32 patch );
                return new SemVer( major, minor, patch );
            }
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            LLVMResetFatalErrorHandler();
            LLVMShutdown();
        }

        /// <summary>Initializes the native LLVM library support</summary>
        /// <returns><see cref="ILibLlvm"/> implementation for the library</returns>
        public static ILibLlvm InitializeLLVM( )
        {
            // If this is the first call and the library resolver is applied, then validate
            // the version from the library for sanity.
            if(!NativeLibraryResolver.Apply())
            {
                throw new InvalidOperationException( "LLVM library was previously initialized. Re-init is not supported in the native library" );
            }

            // Verify the version of LibLLVM.
            var libVersion = SemVer.Parse(LibLLVMGetVersion()?.ToString() ?? string.Empty, SemVerFormatProvider.CaseInsensitive);
            if( libVersion is CSemVerCI semVerCI)
            {
                libVersion = semVerCI.BaseBuild;
            }

            if( libVersion.Major != SupportedVersion.Major
             || libVersion.Minor != SupportedVersion.Minor
             || libVersion.Patch != SupportedVersion.Patch
            )
            {
                string msgFmt = Resources.Mismatched_LibLLVM_version_Expected_0_Actual_1;
                string msg = string.Format( CultureInfo.CurrentCulture
                                          , msgFmt
                                          , SupportedVersion.ToString()
                                          , libVersion.ToString()
                                          );

                throw new InvalidOperationException( msg );
            }

            return new Library(libVersion);
        }

        private Library( SemVer libVersion )
        {
            ExtendedAPIVersion = libVersion;
            unsafe
            {
                LLVMInstallFatalErrorHandler( &FatalErrorHandler );
            }
        }

        private readonly Lazy<ImmutableArray<LibLLVMCodeGenTarget>> LazyTargets = new(GetSupportedTargets);

        // Expected version info for verification of matched LibLLVM
        // Interop exports/signatures may not be valid if not a match.
        private static readonly CSemVer SupportedVersion = new(20, 1, 8);

        // Singleton initializer for the supported targets array
        private static ImmutableArray<LibLLVMCodeGenTarget> GetSupportedTargets( )
        {
            var resultArray = new LibLLVMCodeGenTarget[LibLLVMGetNumTargets()];
            LibLLVMGetRuntimeTargets( resultArray ).ThrowIfFailed();

            // Create a new immutable array without copy (Wraps the input array)
            return ImmutableCollectionsMarshal.AsImmutableArray( resultArray );
        }

        // Native call back for fatal error handling.
        // NOTE: LLVM will call exit() upon return from this function and there's no way to stop it
        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe void FatalErrorHandler( byte* reason )
        {
            try
            {
                Trace.TraceError( "LLVM Fatal Error: '{0}'; Application will exit.", ExecutionEncodingStringMarshaller.ConvertToManaged( reason ) );
            }
            catch(Exception ex)
            {
                // No finalizers will occur after this, it's a HARD termination of the app.
                // LLVM will do that on return but this can at least indicate a different problem
                // from the original one LLVM was reporting.
                Environment.FailFast( $"Unhandled exception in {nameof( FatalErrorHandler )}.", ex );
            }
        }
    }
}
