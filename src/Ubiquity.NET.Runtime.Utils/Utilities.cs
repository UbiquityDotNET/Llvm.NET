// -----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Ubiquity.NET.Runtime.Utils
{
    public static class Utilities
    {
        /// <summary>replaces any characters in a name that are invalid for a file name</summary>
        /// <param name="name">name to convert</param>
        /// <param name="replacementChar">character to replace invalid characters with [Default: '_']</param>
        /// <returns><paramref name="name"/> with invalid characters replaced with <paramref name="replacementChar"/></returns>
        public static string GetSafeFileName( string name, char replacementChar = '_' )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var bldr = new StringBuilder( name.Length );
            char[ ] invalidChars = Path.GetInvalidFileNameChars( );
            foreach( char c in name )
            {
                bldr.Append( invalidChars.Contains( c ) ? replacementChar : c );
            }

            return bldr.ToString( );
        }
    }
}
