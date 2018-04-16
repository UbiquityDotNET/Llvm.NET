// <copyright file="GeneratedCodeExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Llvm.NET.Native
{
    internal static partial class NativeMethods
    {
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

            var searchCookies = ( from path in alternatePaths
                                  where Directory.Exists( path )
                                  select ( Cookie: AddDllDirectory( path ), Path: path )
                                ).ToList( );

            try
            {
                IntPtr moduleHandle = LoadLibraryExW( moduleName, IntPtr.Zero, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS );
                if( moduleHandle == IntPtr.Zero )
                {
                    int lasterror = Marshal.GetLastWin32Error( );
                    string errMessage = $"System error occurred trying to load DLL {moduleName}.\n Search paths:\n {string.Join("\n", searchCookies.Select(p=>p.Path))}";
                    throw new Win32Exception( lasterror, errMessage );
                }

                return moduleHandle;
            }
            finally
            {
                foreach( var c in searchCookies )
                {
                    RemoveDllDirectory( c.Cookie );
                }
            }
        }

        [DllImport( "kernel32", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool FreeLibrary( IntPtr hModule );

        private const UInt32 LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;
        private const UInt32 LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400;

        [DllImport( "kernel32", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern IntPtr LoadLibraryExW( [MarshalAs( UnmanagedType.LPTStr )]string lpFileName, IntPtr hFile, UInt32 dwFlags );

        [DllImport( "kernel32", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern IntPtr AddDllDirectory( [MarshalAs( UnmanagedType.LPWStr )]string lp );

        [DllImport( "kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RemoveDllDirectory( IntPtr dwCookie );
    }
}
