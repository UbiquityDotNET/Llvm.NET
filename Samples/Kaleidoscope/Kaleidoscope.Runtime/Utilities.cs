// -----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using Ubiquity.NET.ArgValidators;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

namespace Kaleidoscope.Runtime
{
    public static class Utilities
    {
        /// <summary>replaces any characters in a name that are invalid for a file name</summary>
        /// <param name="name">name to convert</param>
        /// <returns>name with invalid characters replaced with '_'</returns>
        public static string GetSafeFileName( string name )
        {
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            var bldr = new StringBuilder( name.Length );
            char[ ] invalidChars = Path.GetInvalidFileNameChars( );
            foreach( char c in name )
            {
                bldr.Append( invalidChars.Contains( c ) ? '_' : c );
            }

            return bldr.ToString( );
        }
    }
}
