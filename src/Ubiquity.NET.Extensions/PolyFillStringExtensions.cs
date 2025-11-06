// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

#if !NET6_0_OR_GREATER

// from .NET sources
// see: https://github.com/dotnet/runtime/blob/1d1bf92fcf43aa6981804dc53c5174445069c9e4/src/libraries/System.Private.CoreLib/src/System/String.Manipulation.cs

#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented [Duplicate of CS1591]

using System.Text.RegularExpressions;

namespace System.Text
{
    /// <summary>Pollyfill extensions for support not present in older runtimes</summary>
    /// <inheritdoc cref="PolyFillExceptionValidators" path="/remarks"/>
    public static class PolyFillStringExtensions
    {
        /// <summary>Replace line endings in the string with environment specific forms</summary>
        /// <param name="self">string to change line endings for</param>
        /// <returns>string with environment specific line endings</returns>
        [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1611:Element parameters should be documented", Justification = "Extension" )]
        public static string ReplaceLineEndings(this string self) => ReplaceLineEndings(self, Environment.NewLine);

        // This is NOT the most performant implementation, it's going for simplistic pollyfill that has
        // the correct behavior, even if not the most performant. If performance is critical, use a
        // later version of the runtime!

        /// <summary>Replace line endings in the string with a given string</summary>
        /// <param name="self">string to change line endings for</param>
        /// <param name="replacementText">Text to replace all of the line endings in <paramref name="self"/></param>
        /// <returns>string with line endings replaced by <paramref name="replacementText"/></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1611:Element parameters should be documented", Justification = "Extension" )]
        public static string ReplaceLineEndings( this string self, string replacementText )
        {
            ArgumentNullException.ThrowIfNull(self);
            ArgumentNullException.ThrowIfNull(replacementText);

            string retVal = UnicodeNewLinesRegEx.Replace(self, replacementText);

            // if the result of replacement is the same, just return the original
            // This is wasted overhead, but at least matches the behavior
            return self == retVal ? self : retVal;
        }

        // The Unicode Standard, Sec. 5.8, Recommendation R4 and Table 5-2 state that the CR, LF,
        // CRLF, NEL, LS, FF, and PS sequences are considered newline functions. That section
        // also specifically excludes VT from the list of newline functions, so we do not include
        // it in the regular expression match.

        // language=regex
        private const string UnicodeNewLinesRegExPattern = @"(\r\n|\r|\n|\f|\u0085|\u2028|\u2029)";

        private static Regex UnicodeNewLinesRegEx { get; } = new Regex( UnicodeNewLinesRegExPattern );
    }
}
#endif
