// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Extensions
{
    /// <summary>Poly fill extensions for the <see cref="StringSplitOptions"/> enumeration</summary>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Extension - Broken analyzer" )]
    [Flags]
    public enum StringSplitOptions2
    {
        /// <summary>Use the default options when splitting strings.</summary>
        None = StringSplitOptions.None,

        /// <summary>Omit array elements that contain an empty string from the result.</summary>
        /// <remarks>
        /// If <see cref="RemoveEmptyEntries"/> and <see cref="TrimEntries"/> are specified together,
        /// then substrings that consist only of white-space characters are also removed from the result.
        /// </remarks>
        RemoveEmptyEntries = StringSplitOptions.RemoveEmptyEntries,

#if NET5_0_OR_GREATER
        /// <summary>Trim white-space characters from each substring in the result.</summary>
        /// <remarks>
        /// If <see cref="RemoveEmptyEntries"/> and <see cref="TrimEntries"/> are specified together,
        /// then substrings that consist only of white-space characters are also removed from the result.
        /// </remarks>
        TrimEntries = StringSplitOptions.TrimEntries
#else
        /// <summary>Trim white-space characters from each substring in the result.</summary>
        /// <remarks>
        /// <note type="important">
        /// The official value of this field is available in .NET 5 and later versions only. Unless a method
        /// using <see cref="StringSplitOptions"/> is explicitily re-written to support <see cref="StringSplitOptions2"/>,
        /// then the functionality for this is not available.
        /// </note>
        /// <para>
        /// If <see cref="RemoveEmptyEntries"/> and <see cref="TrimEntries"/> are specified together,
        /// then substrings that consist only of white-space characters are also removed from the result.
        /// </para>
        /// </remarks>
        TrimEntries = 2,
#endif
    }
}
