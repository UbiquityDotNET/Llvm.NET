// -----------------------------------------------------------------------
// <copyright file="LlvmStringHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Abstract base class for types that represents an LLVM string</summary>
    /// <remarks>
    /// This base class provides most of the functionality for a string pointer except
    /// the disposal/release of the string. That is left to derived types to provide the
    /// specific operation to release the pointer.
    /// </remarks>
    public abstract class LlvmStringHandle
        : SafeHandle
    {
        /// <inheritdoc/>
        public override bool IsInvalid => handle == nint.Zero;

        /// <summary>Creates a readonly span for the data in this string</summary>
        /// <returns>Span of the ANSI characters in this string (as byte)</returns>
        public ReadOnlySpan<byte> CreateReadOnlySpan()
        {
            if(IsClosed || IsInvalid)
            {
                return new();
            }

            unsafe
            {
                return MemoryMarshal.CreateReadOnlySpanFromNullTerminated( (byte*)handle );
            }
        }

        /// <summary>Converts the underlying ANSI pointer into a managed string</summary>
        /// <returns>Managed string for the provided pointer</returns>
        /// <remarks>
        /// The return is a managed string that is equivalent to the string of this pointer.
        /// It's lifetime is controlled by the runtime GC.
        /// </remarks>
        public override string? ToString()
        {
            return IsClosed || IsInvalid ? null : Marshal.PtrToStringAnsi( handle );
        }

        private protected LlvmStringHandle()
            : base( nint.Zero, ownsHandle: true )
        {
        }

        private protected LlvmStringHandle(nint p)
            : this()
        {
            SetHandle( p );
        }

        private protected unsafe LlvmStringHandle(byte* p)
            : this( (nint)p )
        {
        }
    }
}
