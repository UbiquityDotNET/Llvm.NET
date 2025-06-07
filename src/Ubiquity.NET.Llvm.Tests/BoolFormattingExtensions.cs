// -----------------------------------------------------------------------
// <copyright file="BoolFormattingExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace Ubiquity.NET.Llvm.UT
{
    // The default conversion of a bool to string uses casing that is inconsistent
    // with the representation in C#, this covers that as a simple extension.
    // Some future variant of this could support generation for different languages
    // using some for of Enum as a discriminator, but there's no need for that in this
    // test assembly...
    internal static class BoolFormattingExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "It MUST be lower case; not 'normalizing' a string here." )]
        public static string ToCSName( this bool value )
        {
            return value.ToString( CultureInfo.InvariantCulture ).ToLowerInvariant();
        }
    }
}
