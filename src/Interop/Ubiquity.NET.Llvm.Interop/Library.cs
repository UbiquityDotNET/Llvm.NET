// <copyright file="StaticState.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System.Reflection;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public sealed partial class Library
        : ILibLlvm
    {
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
            var previousState = (InitializationState)Interlocked.CompareExchange( ref CurrentInitializationState
                                                                                , (int)InitializationState.Initializing
                                                                                , (int)InitializationState.Uninitialized
                                                                                );
            if(previousState != InitializationState.Uninitialized && previousState != InitializationState.ShutDown )
            {
                throw new InvalidOperationException( Resources.Llvm_already_initialized );
            }

            // only set a resolver once. Multiple attempts results in an exception
            if(!Interlocked.Exchange(ref ResolverApplied, true))
            {
                NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), NativeLibResolver);
            }

            // Verify the version of LLVM in LibLLVM
            LibLLVMGetVersionInfo( out LibLLVMVersionInfo versionInfo );
            if(versionInfo.Major != VersionMajor
                || versionInfo.Minor != VersionMinor
                || versionInfo.Patch < VersionPatch
                )
            {
                string msgFmt = Resources.Mismatched_LibLLVM_version_Expected_0_1_2_Actual_3_4_5;
                string msg = string.Format( CultureInfo.CurrentCulture
                                            , msgFmt
                                            , VersionMajor
                                            , VersionMinor
                                            , VersionPatch
                                            , versionInfo.Major
                                            , versionInfo.Minor
                                            , versionInfo.Patch
                                            );

                throw new InvalidOperationException( msg );
            }

            Interlocked.Exchange( ref CurrentInitializationState, (int)InitializationState.Initialized );
            return new Library( );
        }

        /// <inheritdoc/>
        [SuppressMessage( "StyleCop.CSharp.LayoutRules", "SA1515:Single-line comment should be preceded by blank line", Justification = "Annoying analyzer for comments on disabled warnings..." )]
        public void Dispose()
        {
            var previousState = (InitializationState)Interlocked.CompareExchange( ref CurrentInitializationState
                                                                                , (int)InitializationState.ShuttingDown
                                                                                , (int)InitializationState.Initialized
                                                                                );
            if(previousState != InitializationState.Initialized)
            {
                Debug.Assert( false, Resources.Llvm_not_initialized );
                return;
            }

            LLVMShutdown();
            Interlocked.Exchange( ref CurrentInitializationState, (int)InitializationState.ShutDown );
        }

        private Library()
        {
        }

        private enum InitializationState
        {
            Uninitialized,
            Initializing,
            Initialized,
            ShuttingDown,
            ShutDown, // NOTE: This is a terminal state, it doesn't return to uninitialized
        }

        // version info for verification of matched LibLLVM
        private const int VersionMajor = 20;
        private const int VersionMinor = 1;
        private const int VersionPatch = 0;

        private static int CurrentInitializationState;

        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe void FatalErrorHandler(byte* reason)
        {
            try
            {
                // NOTE: LLVM will call exit() upon return from this function and there's no way to stop it
                Trace.TraceError( "LLVM Fatal Error: '{0}'; Application will exit.", ExecutionEncodingStringMarshaller.ConvertToManaged( reason ) );
            }
            catch
            {
            }
        }

        private static nint NativeLibResolver( string libraryName, Assembly assembly, DllImportSearchPath? searchPath )
        {
            // Any library other than the one known about here gets default handling
            if (libraryName != LibraryName)
            {
                return nint.Zero;
            }

            if(NativeLibHandle.IsInvalid || NativeLibHandle.IsClosed)
            {
                // Native binary is in a RID specific runtime folder, build that path as relative
                // to this assembly and load the library from there.
                string relativePath = @$"runtimes/{RuntimeInformation.RuntimeIdentifier}/native/{libraryName}";
                NativeLibHandle = NativeLibraryHandle.Load(relativePath, assembly, DllImportSearchPath.AssemblyDirectory);

                unsafe
                {
                    LLVMInstallFatalErrorHandler( &FatalErrorHandler );
                }
            }

            return NativeLibHandle.DangerousGetHandle();
        }

        private static bool ResolverApplied = false;
        private static NativeLibraryHandle NativeLibHandle = new();
    }
}
