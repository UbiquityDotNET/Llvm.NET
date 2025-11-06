// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.SrcGeneration
{
    /// <summary>Utility class to host extensions for a <see cref="string"/>.</summary>
    public static class StringExtensions
    {
        /// <summary>Gets the lines of this instance as a contents for a comment</summary>
        /// <param name="self">String to apply extension to</param>
        /// <param name="options">String split options to use when splitting lines</param>
        /// <returns>Enumerable sequence of comment strings without any language specific leading or trailing delimiters</returns>
        /// <remarks>
        /// This is generally used by language specific extensions that will also emit the comment leading/trailiing text as needed.
        /// Thus it is a general language neutral facility that is used to produce the final language specific comments.
        /// It will perform the following on the input string:
        /// 1) <see cref="EscapeComment(string)"/> to ensure character escaping is applied to the whole string<br/>
        /// 2) <see cref="SplitLines(string, StringSplitOptions2)"/>to split the string into distinct lines<br/>
        /// 3) <see cref="EscapeForXML(IEnumerable{string})"/> to ensure the lines are valid for XML doc comments<br/>
        /// 4) <see cref="SkipDuplicates(IEnumerable{string})"/> to fold duplicate entries into one (usually to reduce multiple blank lines to one)<br/>
        /// </remarks>
        public static IEnumerable<string> GetCommentLines( this string self, StringSplitOptions2 options = StringSplitOptions2.TrimEntries )
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
#if NETSTANDARD2_0
            return self.Replace( "\\n", Environment.NewLine );
#else
            return self.Replace( "\\n", Environment.NewLine, StringComparison.Ordinal );
#endif
        }

        /// <summary>Splits a string into a sequence of lines</summary>
        /// <param name="self">String to apply extension to</param>
        /// <param name="splitOptions">String split options to use when splitting lines</param>
        /// <returns>Enumerable sequence of strings, one for each line in the original</returns>
        /// <remarks>
        /// <note type="important">
        /// For runtimes prior to .NET 5 the behavior of <see cref="StringSplitOptions2.TrimEntries"/>
        /// is emulated here. The implementation of that emulation is not as performant as the
        /// official form in later runtimes. This emulation was chosen for correctness of behavior
        /// and simplicity of implementation over performance. If absolute best performance is
        /// desired then use the latest runtime.
        /// </note>
        /// </remarks>
        public static IEnumerable<string> SplitLines( this string self, StringSplitOptions2 splitOptions = StringSplitOptions2.None )
        {
            ArgumentNullException.ThrowIfNull( self );

#if !NET5_0_OR_GREATER
            // StringSplitOptions.TrimeEntries member is not available, do it the hard/slow way
            if(splitOptions.HasFlag(StringSplitOptions2.TrimEntries))
            {
                var options = (StringSplitOptions)((int)splitOptions & ~(int)StringSplitOptions2.TrimEntries);
                return from s in self.Split( MixedLineEndings, options)
                       let t = s.Trim()
                       where !splitOptions.HasFlag(StringSplitOptions2.RemoveEmptyEntries) || !string.IsNullOrEmpty(t)
                       select t;
            }
#endif
            return self.Split( MixedLineEndings, (StringSplitOptions)splitOptions);
        }

        // TODO: WithLines(Action<ReadOnlySpan<char>> op)
        //    finds all line endings and provides each line as a span to op
        //    This avoids the problem of IEnumerable<ReadOnlySpan<char>> lifetime management
        //    though that is plausible with a custom implementation instead of a generated
        //    iterator...

        /// <summary>Escapes a string for use in XML</summary>
        /// <param name="self">String to apply extension to</param>
        /// <returns>XML safe escaped string</returns>
        /// <remarks>This will perform escaping of characters for XML such as conversion of `&amp;` into `&amp;amp;` etc...</remarks>
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
        /// <para>For the purposes of this method `duplicate` means "identical to previous". Thus,
        /// it is possible for multiple entries in <paramref name="self"/> to have the same value
        /// in the result, just none where the previous entry is the same as the current one. This
        /// is normally used when importing from a format that contains multiple new lines in a
        /// row so they are converted to a single new line.</para>
        /// <para>The returned value is a deferred iterator, the actual work of duplicate detection
        /// and removal is deferred until needed by the caller and stopped whenever the caller ceases
        /// to enumerate more items.</para>
        /// </remarks>
        public static IEnumerable<string> SkipDuplicates( this IEnumerable<string> self )
        {
            ArgumentNullException.ThrowIfNull( self );

            string? oldVal = null;
            return self.Where((s)=>
                {
                    bool retVal = s != oldVal;
                    oldVal = s;
                    return retVal;
                } );
        }

#pragma warning disable IDE0002
// names can't be simplified further due to weird ambiguities with how extensions are resolved
// net stadard 2.0 does NOT contain the static methods for argument validation on exceptions.
// Thus in tat runtime they are polly fill extensions, but they have the same name as instance
// extensions - those win out and collide causing mass confusion.
        private static readonly string [] MixedLineEndings =
            [
                Extensions.StringNormalizer.LineEnding(Extensions.LineEndingKind.CarriageReturnLineFeed),
                Extensions.StringNormalizer.LineEnding(Extensions.LineEndingKind.CarriageReturn),
                Extensions.StringNormalizer.LineEnding(Extensions.LineEndingKind.LineFeed),
            ];
#pragma warning restore IDE0002
    }
}
