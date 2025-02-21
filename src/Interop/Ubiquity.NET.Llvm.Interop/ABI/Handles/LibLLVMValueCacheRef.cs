// -----------------------------------------------------------------------
// <copyright file="LibLLVMValueCacheRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Global LLVM object handle</summary>
    [SuppressMessage( "Design", "CA1060:Move pinvokes to native methods class", Justification = "ONLY used by this class" )]
    public class LibLLVMValueCacheRef
        : LlvmObjectRef
    {
        /// <summary>Initializes an instance of <see cref="LibLLVMValueCacheRef"/> with default values</summary>
        public LibLLVMValueCacheRef()
            : base( ownsHandle: true )
        {
        }

        /// <summary>Initializes an instance of <see cref="LibLLVMValueCacheRef"/></summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        /// <param name="owner">Value to indicate whether the handle is owned or not</param>
        public LibLLVMValueCacheRef(nint handle, bool owner)
            : base( owner )
        {
            SetHandle( handle );
        }

        /// <summary>Gets a Zero (<see langword="null"/>) value handle</summary>
        public static LibLLVMValueCacheRef Zero { get; } = new LibLLVMValueCacheRef( nint.Zero, false );

        /// <inheritdoc/>
        protected override bool ReleaseHandle()
        {
            // critical safety check, base should never call ReleaseHandle on an invalid handle
            // but ABI usually can't handle that and would just crash the app, so make it
            // a NOP just in case.
            if(handle != nint.Zero)
            {
                LibLLVMDisposeValueCache( handle );
            }

            return true;

            [DllImport( NativeMethods.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LibLLVMDisposeValueCache(nint p);
        }
    }
}
