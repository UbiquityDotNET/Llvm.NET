// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.SrcGeneration
{
    // TODO: What version of the language?

    /// <summary>Target language for creating identifier symbols</summary>
    public enum TargetLang
    {
        /// <summary>Use formatting for C# language</summary>
        CSharp = 0,

        /// <summary>Use formatting for VB.Net language</summary>
        VisualBasic = 1,

        /// <summary>Use formatting for F# language</summary>
        FSharp = 2,
    }

    /// <summary>Utility class to host extensions for a <see cref="string"/>.</summary>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "extension" )]
    [SuppressMessage( "Naming", "CA1708:Identifiers should differ by more than case", Justification = "extension" )]
    [SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "extension" )]
    public static class StringExtensions
    {
        /// <summary>Gets the lines of this instance as a comment</summary>
        /// <param name="self">String to apply extension to</param>
        /// <param name="options">String split options to use when splitting lines</param>
        /// <returns>Enumerable sequence of comment strings without any comment prefix (That is, no language specific leading'//', '/*' etc...)</returns>
        public static IEnumerable<string> GetCommentLines( this string self, StringSplitOptions options = StringSplitOptions.TrimEntries )
        {
            ArgumentNullException.ThrowIfNull( self );

            // For now, naive conversion - just splits on newlines
            // more sophisticated implementation could split on word boundaries based on length...
            return self.EscapeComment()
                        .SplitLines( options )
                        .EscapeForXML()
                        .SkipDuplicates();
        }

        /// <summary>Escapes characters in a comment</summary>
        /// <param name="self">String to apply extension to</param>
        /// <returns>Escaped comment</returns>
        /// <remarks>
        /// Currently, the only character that is allowed for an escape is '\n'
        /// which is converted to an <see cref="Environment.NewLine"/>.
        /// </remarks>
        public static string EscapeComment( this string self )
        {
            ArgumentNullException.ThrowIfNull( self );

            // For now, the only escape is a newline '\n'
            return self.Replace( "\\n", Environment.NewLine, StringComparison.Ordinal );
        }

        /// <summary>Splits a string into a sequence of lines</summary>
        /// <param name="self">String to apply extension to</param>
        /// <param name="splitOptions">String split options to use when splitting lines</param>
        /// <returns>Enumerable sequence of strings, one for each line in the original</returns>
        public static string[] SplitLines( this string self, StringSplitOptions splitOptions = StringSplitOptions.None )
        {
            ArgumentNullException.ThrowIfNull( self );
            return self.Split( MixedLineEndings, splitOptions );
        }

        // TODO: WithLines(Action<ReadOnlySpan<char>> op)
        //    finds all line endings and provides each as a span to op
        //    This avoids the problem of IEnumerable<ReadOnlySpan<char>> lifetime management
        //    though that is plausible with a custom implementation instead of a generated
        //    iterator...

        /// <summary>Escapes a string for use in XML</summary>
        /// <param name="self">String to apply extension to</param>
        /// <returns>XML safe escaped string</returns>
        public static string MakeXmlSafe( this string self )
        {
            ArgumentNullException.ThrowIfNull( self );
            return new XText( self ).ToString();
        }

        /// <summary>Transforms a sequence of strings to a sequence of XML escaped strings</summary>
        /// <param name="self">Sequence to apply this extension to</param>
        /// <returns>Sequence of XML escaped strings</returns>
        public static IEnumerable<string> EscapeForXML( this IEnumerable<string> self )
        {
            ArgumentNullException.ThrowIfNull( self );

            return from s in self
                   select MakeXmlSafe( s );
        }

        /// <summary>Transforms a sequence of strings to a sequence of strings escaped for comments</summary>
        /// <param name="self">Sequence to apply this extension to</param>
        /// <returns>Sequence of escaped strings</returns>
        /// <seealso cref="EscapeComment(string?)"/>
        public static IEnumerable<string> EscapeForComment( this IEnumerable<string> self )
        {
            ArgumentNullException.ThrowIfNull( self );

            return from s in self
                   select EscapeComment( s );
        }

        /// <summary>Transforms a sequence of strings removing duplicate strings</summary>
        /// <param name="self">Sequence to apply this extension to</param>
        /// <returns>Sequence of strings that has no duplicate side by side entries</returns>
        /// <remarks>
        /// For the purposes of this method `duplicate` means "identical to previous". Thus,
        /// it is possible for multiple entries to have the same value in the result, just none
        /// where the previous entry is the same as the current one. This is normally used to
        /// when importing from a format that contains multiple new lines in a row so they are
        /// converted to a single new line.
        /// </remarks>
        public static IEnumerable<string> SkipDuplicates( this IEnumerable<string> self )
        {
            string? oldVal = null;
            foreach(string val in self)
            {
                if(val != oldVal)
                {
                    yield return val;
                }

                oldVal = val;
            }
        }

        private static readonly string [] MixedLineEndings =
            [
                LineEndingKind.CarriageReturnLineFeed.LineEnding(),
                LineEndingKind.CarriageReturn.LineEnding(),
                LineEndingKind.LineFeed.LineEnding(),
            ];
    }
}
