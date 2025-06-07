// -----------------------------------------------------------------------
// <copyright file="NativeLibraryHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Safe handle for a <see cref="NativeLibrary"/></summary>
    public sealed class NativeLibraryHandle
        : SafeHandle
    {
        /// <summary>Initializes a new instance of the <see cref="NativeLibraryHandle"/> class.</summary>
        public NativeLibraryHandle( )
            : base( 0, true )
        {
        }

        /// <inheritdoc/>
        public override bool IsInvalid => handle == 0;

        /// <inheritdoc cref="NativeLibrary.Load(string, Assembly, DllImportSearchPath?)"/>
        public static NativeLibraryHandle Load( string libraryName, Assembly assembly, DllImportSearchPath? searchPath )
        {
            return new( NativeLibrary.Load( libraryName, assembly, searchPath ) );
        }

        /// <inheritdoc cref="NativeLibrary.TryLoad(string, out nint)"/>
        /// <param name="lib">Native library handle.</param>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        public static bool TryLoad( string libraryPath, [MaybeNullWhen( false )] out NativeLibraryHandle lib )
        {
            if(NativeLibrary.TryLoad( libraryPath, out nint handle ))
            {
                lib = new NativeLibraryHandle( handle );
                return true;
            }

            lib = null;
            return false;
        }

        /// <inheritdoc cref="NativeLibrary.TryLoad(string, Assembly, DllImportSearchPath?, out nint)"/>
        /// <param name="lib">Native library Handle</param>
        public static bool TryLoad( string libraryName, Assembly assembly, DllImportSearchPath? searchPath, [MaybeNullWhen( false )] out NativeLibraryHandle lib )
        {
            if(NativeLibrary.TryLoad( libraryName, assembly, searchPath, out nint handle ))
            {
                lib = new NativeLibraryHandle( handle );
                return true;
            }

            lib = null;
            return false;
        }
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

        /// <inheritdoc/>
        protected override bool ReleaseHandle( )
        {
            NativeLibrary.Free( handle );
            return true;
        }

        private NativeLibraryHandle( nint osHandle )
            : this()
        {
            SetHandle( osHandle );
        }
    }
}
