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

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Interop methods for the Ubiquity.NET LibLLVM library</summary>
    public static partial class NativeMethods
    {
        internal const string LibraryPath = "Ubiquity.NET.LibLLVM";

        /// <summary>Dynamically loads a DLL from a directory dependent on the current architecture</summary>
        /// <param name="moduleName">name of the DLL</param>
        /// <param name="alternatePaths">alternate path locations to use to search for the DLL</param>
        /// <returns>Handle for the DLL</returns>
        internal static IntPtr LoadWin32Library( string moduleName, IEnumerable<string> alternatePaths )
        {
            if( string.IsNullOrWhiteSpace( moduleName ) )
            {
                throw new ArgumentNullException( nameof( moduleName ) );
            }

            foreach ( string path in alternatePaths )
            {
                string? modulePath = Path.Combine( path, moduleName );
                try
                {
                    var moduleHandle = NativeLibrary.Load( moduleName );
                    if ( moduleHandle != IntPtr.Zero )
                    {
                        return moduleHandle;
                    }
                }
                catch ( DllNotFoundException _ )
                {
                }
            }

            string locations = string.Join( "\n", alternatePaths );
            string msgFmt = Resources.LoadLibrary_not_found;
            string errMessage = string.Format( CultureInfo.CurrentCulture, msgFmt, moduleName, locations );
            throw new DllNotFoundException( errMessage );

            //var searchCookies = ( from path in alternatePaths
            //                      where Directory.Exists( path )
            //                      select (Cookie: AddDllDirectory( path ), Path: path)
            //                    ).ToList( );

            //try
            //{
            //    IntPtr moduleHandle = LoadLibraryExW( moduleName, IntPtr.Zero, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS );
            //    if( moduleHandle != IntPtr.Zero )
            //    {
            //        return moduleHandle;
            //    }

            //    int lastError = Marshal.GetLastWin32Error( );
            //    string msgFmt = Resources.LoadWin32Library_Error_0_occured_loading_1_search_paths_2;
            //    string errMessage = string.Format( CultureInfo.CurrentCulture, msgFmt, lastError, moduleName, string.Join( "\n", searchCookies.Select( p => p.Path ) ) );
            //    throw new Win32Exception( lastError, errMessage );
            //}
            //finally
            //{
            //    foreach( var c in searchCookies )
            //    {
            //        RemoveDllDirectory( c.Cookie );
            //    }
            //}
        }

        [DllImport( "kernel32", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool FreeLibrary( IntPtr hModule );

        private const UInt32 LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;
        /* private const UInt32 LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400; */

        [DllImport( "kernel32", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern IntPtr LoadLibraryExW( [MarshalAs( UnmanagedType.LPTStr )]string lpFileName, IntPtr hFile, UInt32 dwFlags );

        [DllImport( "kernel32", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern IntPtr AddDllDirectory( [MarshalAs( UnmanagedType.LPWStr )]string lp );

        [DllImport( "kernel32", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static extern bool RemoveDllDirectory( IntPtr dwCookie );
    }
}
