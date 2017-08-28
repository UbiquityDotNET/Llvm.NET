using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Llvm.NET.Native
{
    internal static partial class NativeMethods
    {
        // version info for verification of matched LibLLVM
        private const int VersionMajor = 4;
        private const int VersionMinor = 0;
        private const int VersionPatch = 1;

        private static void FatalErrorHandler( string Reason )
        {
            // NOTE: LLVM will call exit() upon return from this function and there's no way to stop it
            Trace.TraceError( "LLVM Fatal Error: '{0}'; Application exiting.", Reason );
        }

        /* TODO: Remove static constructor in favor of a static state init utility that returns IDisposable
           so that all the analyzer issue here go away and proper LLVMShutdown can be performed.
        */

        /// <summary>Static constructor for NativeMethods</summary>
        [SuppressMessage( "Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "Exception needed as code can't function if LibLLVM isn't loaded" )]
        [SuppressMessage( "Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Constructor needed to run init code" )]
        static NativeMethods( )
        {
            // force loading the appropriate architecture specific
            // DLL before any use of the wrapped inter-op APIs to
            // allow building this library as ANYCPU
            string path = Path.GetDirectoryName( Assembly.GetExecutingAssembly( ).Location );
            if( Directory.Exists( Path.Combine( path, "LibLLVM" ) ) )
            {
                LoadWin32Library( libraryPath, "LibLLVM" );
            }
            else
            {
                // fall-back to standard library search paths to allow building
                // CPU specific variants with only one native DLL without needing
                // conditional compilation on this library, which is useful for
                // unit testing or whenever the Nuget packaging isn't desired.
                LoadWin32Library( libraryPath, null );
            }

            // Verify the version of LLVM in LibLLVM
            GetVersionInfo( out LLVMVersionInfo versionInfo );
            if( versionInfo.Major != VersionMajor
             || versionInfo.Minor != VersionMinor
             || versionInfo.Patch < VersionPatch
              )
            {
                throw new BadImageFormatException( "Mismatched LibLLVM version" );
            }

            // initialize the static fields
            FatalErrorHandlerDelegate = new Lazy<LLVMFatalErrorHandler>( ( ) => FatalErrorHandler, LazyThreadSafetyMode.PublicationOnly );
            InstallFatalErrorHandler( FatalErrorHandlerDelegate.Value );
        }

        /// <summary>Dynamically loads a DLL from a directory dependent on the current architecture</summary>
        /// <param name="moduleName">name of the DLL</param>
        /// <param name="rootPath">Root path to find the DLL from</param>
        /// <returns>Handle for the DLL</returns>
        /// <remarks>
        /// <para>This method will detect the architecture the code is executing on (i.e. x86 or x64)
        /// and will load the DLL from an architecture specific sub folder of <paramref name="rootPath"/>.
        /// This allows use of AnyCPU builds and interop to simplify build processes from needing to
        /// deal with "mixed" configurations or other accidental combinations that are a pain to
        /// sort out and keep straight when the tools insist on creating AnyCPU projects and "mixed" configurations
        /// by default.</para>
        /// <para>If the <paramref name="rootPath"/>Is <see langword="null"/>, empty or all whitespace then
        /// the standard DLL search paths are used. This assumes the correct variant of the DLL is available
        /// (e.g. for a 32 bit system a 32 bit native DLL is found). This allows for either building as AnyCPU
        /// plus shipping multiple native DLLs, or building for a specific CPU type while shipping only one native
        /// DLL. Different products or projects may have different needs so this covers those cases.
        /// </para>
        /// </remarks>
        internal static IntPtr LoadWin32Library( string moduleName, string rootPath )
        {
            if( string.IsNullOrWhiteSpace( moduleName ) )
            {
                throw new ArgumentNullException( nameof( moduleName ) );
            }

            string libPath;
            if( string.IsNullOrWhiteSpace( rootPath ) )
            {
                libPath = moduleName;
            }
            else
            {
                if( Environment.Is64BitProcess )
                {
                    libPath = Path.Combine( rootPath, "x64", moduleName );
                }
                else
                {
                    libPath = Path.Combine( rootPath, "x86", moduleName );
                }
            }

            IntPtr moduleHandle = LoadLibrary( libPath );
            if( moduleHandle == IntPtr.Zero )
            {
                int lasterror = Marshal.GetLastWin32Error( );
                throw new Win32Exception( lasterror, $"System error occurred trying to load DLL {libPath} from module '{Path.GetDirectoryName( Assembly.GetExecutingAssembly( ).Location )}' " );
            }

            return moduleHandle;
        }

        [DllImport( "kernel32", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern IntPtr LoadLibrary( [MarshalAs( UnmanagedType.LPStr )]string lpFileName );

        // lazy initialized singleton unmanaged delegate so it is never collected
        private static Lazy<LLVMFatalErrorHandler> FatalErrorHandlerDelegate;
    }
}
