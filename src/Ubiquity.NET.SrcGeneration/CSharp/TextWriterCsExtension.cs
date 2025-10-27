// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Ubiquity.NET.SrcGeneration.CSharp
{
    /// <summary>Utility extensions for a <see cref="TextWriter"/> specific to the C# language</summary>
    [SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "extension" )]
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "extension" )]
    public static class TextWriterCsExtension
    {
        /// <summary>Writes an attribute as a line</summary>
        /// <param name="self">The writer to write to</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="attribArgs">arguments for the attribute</param>
        public static void WriteAttributeLine( this TextWriter self, string attributeName, params string[] attribArgs )
        {
            ArgumentNullException.ThrowIfNull( self );

            self.WriteAttribute( attributeName, attribArgs );
            self.WriteLine();
        }

        /// <summary>Writes an attribute to the specified writer</summary>
        /// <param name="self">The writer to write to</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="attribArgs">arguments for the attribute</param>
        public static void WriteAttribute(this TextWriter self, string attributeName, params string[] attribArgs )
        {
            ArgumentNullException.ThrowIfNull( self );

            self.Write( $"[{attributeName}" );
            if(attribArgs.Length > 0)
            {
                self.Write( $"({string.Join( ",", attribArgs )})" );
            }

            self.Write( "]" );
        }

        /// <summary>Writes an XML Doc comment summary</summary>
        /// <param name="self">The writer to write to</param>
        /// <param name="description">Text to include in the summary (Nothing is written if this is <see langword="null"/> </param>
        public static void WriteSummaryComment(this TextWriter self, string? description )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(!string.IsNullOrWhiteSpace( description ))
            {
                self.WriteLine( $"/// <summary>{description!.Trim()}</summary>" );
            }
        }

        /// <summary>Writes remarks comment (XML Doc comment)</summary>
        /// <param name="self">The writer to write to</param>
        /// <param name="txt">Text for the remarks</param>
        /// <remarks>
        /// If <see cref="string.IsNullOrWhiteSpace(string?)"/> for <paramref name="txt"/> is true
        /// then nothing is generated.
        /// </remarks>
        public static void WriteRemarksComment( this TextWriter self, string? txt )
        {
            ArgumentNullException.ThrowIfNull( self );
            if(string.IsNullOrWhiteSpace( txt ))
            {
                return;
            }

            string[] lines = [ .. txt.GetCommentLines() ];
            if(lines.Length > 0)
            {
                self.WriteLine( "/// <remarks>" );
                foreach(string line in lines)
                {
                    self.WriteLine( $"/// {line}" );
                }

                self.WriteLine( "/// </remarks>" );
            }
        }

        /// <summary>Writes summary and remarks comment (XML Doc comment)</summary>
        /// <param name="self">The writer to write to</param>
        /// <param name="txt">Text for the remarks</param>
        /// <param name="defaultSummary">Default summary text to use if <paramref name="txt"/> does not contain any</param>
        /// <remarks>
        /// If <see cref="string.IsNullOrWhiteSpace(string?)"/> for <paramref name="txt"/> is true
        /// then nothing is generated. If <paramref name="txt"/> has no content then <paramref name="defaultSummary"/>
        /// is used as the summary. If <paramref name="defaultSummary"/> is also empty or all Whitespace then nothing
        /// is output.
        /// </remarks>
        public static void WriteSummaryAndRemarksComments( this TextWriter self, string? txt, string defaultSummary = "" )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(string.IsNullOrWhiteSpace( txt ))
            {
                if(!string.IsNullOrWhiteSpace( defaultSummary ))
                {
                    self.WriteLine( $"/// <summary>{defaultSummary}</summary>" );
                }

                return;
            }

            self.WriteLine( $"/// <summary>{defaultSummary}</summary>" );
            string[] lines = [ .. txt.GetCommentLines() ];
            if(lines.Length > 0)
            {
                // summary + remarks.
                self.WriteLine( "/// <remarks>" );
                for(int i = 0; i < lines.Length; ++i)
                {
                    self.Write( "/// " );
                    self.WriteLine( lines[ i ] );
                }

                self.WriteLine( "/// </remarks>" );
            }
        }

        /// <summary>Writes a C# using directive (namespace reference)</summary>
        /// <param name="self">The writer to write to</param>
        /// <param name="namespaceName">Namespace for the using directive</param>
        public static void WriteUsingDirective(this TextWriter self, string namespaceName )
        {
            self.WriteLine( $"using {namespaceName};" );
        }
    }
}
