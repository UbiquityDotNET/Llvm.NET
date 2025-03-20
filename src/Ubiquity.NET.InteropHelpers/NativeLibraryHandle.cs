// -----------------------------------------------------------------------
// <copyright file="NativeLibraryHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Safe handle for a <see cref="NativeLibrary"/></summary>
    public sealed class NativeLibraryHandle
        : SafeHandle
    {
        /// <summary>Initializes a new instance of the <see cref="NativeLibraryHandle"/> class.</summary>
        public NativeLibraryHandle()
            : base(0, true)
        {
        }

        /// <inheritdoc/>
        public override bool IsInvalid => handle == 0;

        /// <inheritdoc cref="NativeLibrary.Load(string, Assembly, DllImportSearchPath?)"/>
        public static NativeLibraryHandle Load(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            return new(NativeLibrary.Load(libraryName, assembly, searchPath));
        }

        /// <inheritdoc/>
        protected override bool ReleaseHandle( )
        {
            NativeLibrary.Free(handle);
            return true;
        }

        private NativeLibraryHandle(nint osHandle)
            : this()
        {
            SetHandle(osHandle);
        }
    }
}
