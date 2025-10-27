// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.Extensions
{
    /// <summary>The kind of line endings for a string of characters</summary>
    public enum LineEndingKind
    {
        /// <summary>Source line endings are mixed or otherwise unknown.</summary>
        /// <remarks>
        /// This kind is only allowed as a source description since it does NOT define any
        /// particular output format. Only an ambiguous input.
        /// </remarks>
        MixedOrUnknownEndings,

        /// <summary>Line endings consist of the single line feed character '\n'</summary>
        /// <remarks>This is the typical form for *nix systems, and Mac OS X and later</remarks>
        LineFeed,

        /// <summary>Line endings consist of a pair of carriage return '\r' AND line feed '\n'</summary>
        /// <remarks>This is the canonical form used in Windows environments</remarks>
        CarriageReturnLineFeed,

        /// <summary>Line endings consist of the single carriage return character '\r'</summary>
        /// <remarks>This is the typical form for older Mac systems</remarks>
        CarriageReturn,
    }

    /// <summary>Utility class for converting line endings to expected forms</summary>
    /// <remarks>This is similar to <see cref="string.ReplaceLineEndings()"/> and
    /// <see cref="string.ReplaceLineEndings(string)"/> except that it allows explicit
    /// control of the input AND output forms of the string via <see cref="LineEndingKind"/>.
    /// Ultimately all forms of normalization resolves to a call to
    /// <see cref="NormalizeLineEndings(string?, LineEndingKind, LineEndingKind)"/>.
    /// </remarks>
    public static partial class StringNormalizer
    {
        /// <summary>Gets a string form of the line ending</summary>
        /// <param name="kind">Kind of line ending to get the string form of</param>
        /// <returns>String form of the specified line ending</returns>
        /// <exception cref="InvalidEnumArgumentException">Unknown value for <paramref name="kind"/></exception>
        public static string LineEnding( this LineEndingKind kind )
        {
            return kind switch
            {
                LineEndingKind.LineFeed => "\n",
                LineEndingKind.CarriageReturnLineFeed => "\r\n",
                LineEndingKind.CarriageReturn => "\r",
                _ => throw new InvalidEnumArgumentException( nameof( kind ), (int)kind, typeof( LineEndingKind ) ),
            };
        }

        /// <summary>Gets the system (<see cref="Environment"/>) line ending kind for the current environment</summary>
        public static LineEndingKind SystemLineEndings => LazySystemKind.Value;

        /// <summary>Normalize a managed string with system defined line endings to a specified kind</summary>
        /// <param name="txt">input string to convert</param>
        /// <param name="dstKind">destination kind of string to convert</param>
        /// <returns>Normalized string; If dstKind matches the current system environment then this returns <paramref name="txt"/> un-modified</returns>
        /// <remarks>
        /// This is equivalent to a call to <see cref="NormalizeLineEndings(string?, LineEndingKind, LineEndingKind)"/> with the
        /// source kind set to <see cref="LineEndingKind.MixedOrUnknownEndings"/>. Thus, it sets all forms of line endings to
        /// the kind specified in <paramref name="dstKind"/>.
        /// </remarks>
        [return: NotNullIfNotNull(nameof(txt))]
        public static string? NormalizeLineEndings( this string? txt, LineEndingKind dstKind )
        {
            return txt.NormalizeLineEndings( LineEndingKind.MixedOrUnknownEndings, dstKind );
        }

        /// <summary>Converts a string into a string with managed environment line endings</summary>
        /// <param name="txt">string to convert</param>
        /// <param name="srcKind">Line ending kind for the source (<paramref name="txt"/>)</param>
        /// <param name="dstKind">Line ending kind for the destination (return string)</param>
        /// <returns>Normalized string; If the <paramref name="srcKind"/> is the same as <paramref name="dstKind"/> this is returns <paramref name="txt"/> un-modified</returns>
        /// <remarks>
        /// Unlike the runtime provided <see cref="string.ReplaceLineEndings(string)"/> this does NOT replace ALL forms
        /// of line endings unless <paramref name="srcKind"/> is <see cref="LineEndingKind.MixedOrUnknownEndings"/>. In
        /// all other cases it ONLY replaces exact matches for the line endings specified in <paramref name="srcKind"/>.
        /// </remarks>
        [return: NotNullIfNotNull(nameof(txt))]
        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Nested conditional are NOT simpler" )]
        public static string? NormalizeLineEndings( this string? txt, LineEndingKind srcKind, LineEndingKind dstKind )
        {
            if(dstKind == LineEndingKind.MixedOrUnknownEndings)
            {
                throw new ArgumentException("Mixed line endings is invalid for the destination.", nameof(dstKind));
            }

            // short-circuit for null, empty, or same kind.
            if(srcKind == dstKind || string.IsNullOrEmpty( txt ))
            {
                return txt;
            }

            return srcKind == LineEndingKind.MixedOrUnknownEndings
                 ? txt.ReplaceLineEndings( dstKind.LineEnding() )
                 : txt.Replace( srcKind.LineEnding(), dstKind.LineEnding(), StringComparison.Ordinal );
        }

        // simplifies consistency of exception in face of unknown environment configuration
        private static InvalidOperationException UnknownLineEndingsException => new( "Unknown environment line ending kind" );

        private static readonly Lazy<LineEndingKind> LazySystemKind = new(ComputeSystemLineEndings);

        [SuppressMessage( "Style", "IDE0066:Convert switch statement to expression", Justification = "Far more readable this way" )]
        private static LineEndingKind ComputeSystemLineEndings( )
        {
            string newLine = Environment.NewLine;
            switch(newLine.Length)
            {
            case 1:
                return newLine[ 0 ] switch
                {
                    CR => LineEndingKind.CarriageReturn,
                    LF => LineEndingKind.LineFeed,
                    _ => throw UnknownLineEndingsException
                };

            case 2:
                return newLine[ 0 ] == CR && newLine[ 1 ] == LF
                     ? LineEndingKind.CarriageReturnLineFeed
                     : throw UnknownLineEndingsException;

            default:
                throw UnknownLineEndingsException;
            }
        }

        private const char LF = '\n';
        private const char CR = '\r';
    }
}
