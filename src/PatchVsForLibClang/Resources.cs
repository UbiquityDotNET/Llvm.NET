// -----------------------------------------------------------------------
// <copyright file="Resources.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace PatchVsForLibClang
{
    internal static class Resources
    {
        // to prevent problems with GIT line ending translation, convert line endings here to the LF form required by the DiffMatchPatch library...
        public static string Intrin0_h_diff => GetResourceStreamText( ResourceNames.Intrin0_h_diff ).Replace("\r\n","\n", StringComparison.Ordinal);

        private static string GetResourceStreamText( string fileName )
        {
            string resName = "PatchVsForLibClang." + fileName;
            var manifestStream = typeof( Resources ).Assembly.GetManifestResourceStream( resName );
            if(manifestStream == null)
            {
                throw new FileNotFoundException( $"Resource {resName} not found!" );
            }

            using var rdr = new StreamReader( manifestStream );
            return rdr.ReadToEnd( );
        }

        private static class ResourceNames
        {
            [SuppressMessage( "Potential Code Quality Issues", "RECS0146:Member hides static member from outer class", Justification = "Intentional" )]
            [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Intentional" )]
            internal const string Intrin0_h_diff = "intrin0_h.diff";
        }
    }
}
