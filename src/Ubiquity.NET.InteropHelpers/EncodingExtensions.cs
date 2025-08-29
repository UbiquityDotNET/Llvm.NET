// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Utility extensions for the <see cref="Encoding"/> class</summary>
    /// <remarks>
    /// These differ from the APIs already present on <see cref="Encoding"/> with
    /// respect to handling of <see langword="null"/> and empty spans. Specifically,
    /// these implementation don't throw an exception in such a case. They just
    /// return <see langword="null"/> when given that as the input. That is, if given
    /// <see langword="null"/> these will return <see langword="null"/> so that the
    /// resulting value mirrors the native value. This is especially important in scenarios
    /// where a <see langword="null"/> value has a distinct meaning from an empty string.
    /// </remarks>
    public static class EncodingExtensions
    {
        /// <summary>Provides conversion of a span of bytes to managed code</summary>
        /// <param name="self">The encoding to use for conversion</param>
        /// <param name="span">Input span to convert with or without a null terminator.</param>
        /// <returns>string containing the decoded characters from the input <paramref name="span"/></returns>
        /// <remarks>
        /// If the input <paramref name="span"/> is empty, then this returns <see langword="null"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="self"/> is <see langword="null"/></exception>
        public static string? MarshalString( this Encoding self, ReadOnlySpan<byte> span )
        {
            ArgumentNullException.ThrowIfNull( self );

            return span.IsEmpty
                 ? string.Empty // optimization for empty spans
                 : self.GetString( span[ ^1 ] == 0 ? span[ ..^1 ] : span ); // drop the null terminator if there is one.
        }

        /// <summary>Provides conversion of the native bytes to managed code</summary>
        /// <param name="self">The encoding to use for conversion</param>
        /// <param name="nativeStringPtr">pointer to the native code string to convert</param>
        /// <returns>string containing the decoded characters from the input <paramref name="nativeStringPtr"/></returns>
        /// <remarks>
        /// If the input <paramref name="nativeStringPtr"/> is <see langword="null"/>, then this returns <see langword="null"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="self"/> is <see langword="null"/></exception>
        public static unsafe string? MarshalString( this Encoding self, byte* nativeStringPtr )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(nativeStringPtr is null)
            {
                return null;
            }

            var span = MemoryMarshal.CreateReadOnlySpanFromNullTerminated( nativeStringPtr );
            return MarshalString( self, span );
        }
    }
}
