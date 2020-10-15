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
        internal static IntPtr LoadWin32Library(string moduleName, IEnumerable<string> alternatePaths)
        {
            if(string.IsNullOrWhiteSpace( moduleName ))
            {
                throw new ArgumentNullException( nameof( moduleName ) );
            }

            // Try the paths we were passed
            foreach(string path in alternatePaths)
            {
                string modulePath = Path.Combine( path, moduleName );
                try
                {
                    IntPtr moduleHandle = NativeLibrary.Load( modulePath );
                    if(moduleHandle != IntPtr.Zero)
                    {
                        return moduleHandle;
                    }
                }
                catch(DllNotFoundException _)
                {
                }
            }

            // Finally, try the default paths
            IntPtr handle = NativeLibrary.Load( moduleName );
            if(handle != IntPtr.Zero)
            {
                return handle;
            }

            string locations = string.Join( "\n", alternatePaths );
            string msgFmt = Resources.LoadLibrary_not_found;
            string errMessage = string.Format( CultureInfo.CurrentCulture, msgFmt, moduleName, locations );
            throw new DllNotFoundException( errMessage );
        }

        internal static void FreeLibrary(IntPtr hModule) => NativeLibrary.Free( hModule );
    }
}
