// -----------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using Ubiquity.NET.Llvm.Interop.Properties;

// TODO: Replace all of this with a Custom import resolver (see: https://learn.microsoft.com/en-us/dotnet/standard/native-interop/native-library-loading#custom-import-resolver)
namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Interop methods for the Ubiquity.NET LibLLVM library</summary>
    public static partial class NativeMethods
    {
        public const string LibraryPath = "Ubiquity.NET.LibLLVM";

        /// <summary>Dynamically loads a library from a directory dependent on the current architecture</summary>
        /// <param name="moduleName">name of the DLL</param>
        /// <param name="alternatePaths">alternate path locations to use to search for the DLL</param>
        /// <returns>Handle for the DLL</returns>
        internal static IDisposable LoadLibrary(string moduleName, IEnumerable<string> alternatePaths)
        {
            if(string.IsNullOrWhiteSpace( moduleName ))
            {
                throw new ArgumentNullException( nameof( moduleName ) );
            }

            var searchCookies = ( from path in alternatePaths
                                  where Directory.Exists( path )
                                  select (Cookie: AddDllDirectory( path ), Path: path)
                                ).ToList( );

            try
            {
                IntPtr moduleHandle = LoadLibraryExW( moduleName, IntPtr.Zero, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS );
                if(moduleHandle != IntPtr.Zero)
                {
                    return new DisposableAction( () => FreeLibrary( moduleHandle ) );
                }

                int lastError = Marshal.GetLastWin32Error( );
                string msgFmt = Resources.LoadWin32Library_Error_0_occured_loading_1_search_paths_2;
                string errMessage = string.Format( CultureInfo.CurrentCulture, msgFmt, lastError, moduleName, string.Join( "\n", searchCookies.Select( p => p.Path ) ) );
                throw new Win32Exception( lastError, errMessage );
            }
            finally
            {
                foreach(var c in searchCookies)
                {
                    RemoveDllDirectory( c.Cookie );
                }
            }
        }

        [LibraryImport( "kernel32", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static unsafe partial bool FreeLibrary(IntPtr hModule);

        private const UInt32 LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;
        /* private const UInt32 LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400; */

        [LibraryImport( "kernel32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16 )]
        private static unsafe partial nint LoadLibraryExW(string lpFileName, IntPtr hFile, UInt32 dwFlags);

        [LibraryImport( "kernel32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16 )]
        private static unsafe partial nint AddDllDirectory(string lp);

        [LibraryImport( "kernel32", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static unsafe partial bool RemoveDllDirectory(IntPtr dwCookie);
    }
}
