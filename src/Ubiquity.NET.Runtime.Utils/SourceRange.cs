// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Ubiquity.NET.Runtime.Utils
{
    // Line number of the starting line [1..n][0 = uninitialized/unknown]
    // Column position of the location [0..n-1]

    /// <summary>Abstraction to hold a source location range as a pair of <see cref="SourcePosition"/> values</summary>
    public readonly record struct SourceRange
        : IFormattable
    {
        /// <summary>Initializes a new instance of the <see cref="SourceRange"/> struct.</summary>
        /// <param name="start">Position for the start of the range</param>
        /// <param name="end">Position for the end of the line</param>
        /// <remarks>
        /// If <paramref name="end"/> is <see langword="default"/> then this creates a simple range
        /// that cannot slice (<see cref="CanSlice"/> is <see langword="false"/>).
        /// </remarks>
        [SetsRequiredMembers]
        public SourceRange( SourcePosition start, SourcePosition end = default )
        {
            Start = start;
            End = end;
        }

        /// <summary>Gets the start position of the range</summary>
        public required SourcePosition Start { get; init; }

        /// <summary>Gets the end position of the range</summary>
        public required SourcePosition End { get; init; }

        /// <summary>Gets a value indicating whether this instance can slice an input</summary>
        /// <remarks>Attempts to slice an input when this is <see langword="false"/></remarks>
        public bool CanSlice => Start.Index.HasValue && End.Index.HasValue;

        /// <summary>Gets the length of the range in characters if available</summary>
        /// <remarks>This only has a value if <see cref="CanSlice"/> is <see langword="true"/></remarks>
        public int? Length => !CanSlice ? null : End.Index - Start.Index - 1;

        /// <inheritdoc/>
        public override string ToString( )
        {
            // use runtime default formatting
            return ToString("R", CultureInfo.CurrentCulture);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Accepted format strings are:
        /// "B" for MSBuild format used for Windows build tools.
        /// "R" for runtime specific [default "B" for Windows]
        /// [Format strings for other runtimes TBD]
        /// </remarks>
        public string ToString( string? format, IFormatProvider? formatProvider )
        {
            formatProvider ??= CultureInfo.CurrentCulture;
            return format switch
            {
                "B" => FormatMsBuild( formatProvider ),
                "R" => FormatRuntime( formatProvider ),
                _ => ToString()
            };
        }

        /// <summary>Forms a slice out of the given read-only span based on this range</summary>
        /// <param name="source">source text to slice</param>
        /// <returns>slice representing the characters of this range</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start or end of the range is not in range of the input source (&lt;0 or &gt;=Length)</exception>
        /// <remarks>
        /// If <see cref="CanSlice"/> is <see langword="false"/> then this returns an empty span. (i.e., does not throw)
        /// </remarks>
        public ReadOnlySpan<char> Slice(ReadOnlySpan<char> source)
        {
            return !CanSlice ? [] : source.Slice(Start.Index!.Value, Length!.Value);
        }

        /// <summary>Implicitly Converts a position to a single point range</summary>
        /// <param name="p">Position to convert</param>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "Implicit operator for public constructor" )]
        public static implicit operator SourceRange( SourcePosition p ) => new( p );

        // SEE: https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-diagnostic-format-for-tasks?view=vs-2022
        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Result is NOT simpler" )]
        private string FormatMsBuild( IFormatProvider formatProvider )
        {
            if(Start.Line == 0)
            {
                return string.Empty;
            }

            if(End.Line == 0)
            {
                return Start.Column == 0
                     ? string.Create(formatProvider, $"({Start.Line}")
                     : string.Create(formatProvider, $"({Start.Line}, {Start.Column})");
            }
            else if(End.Line == Start.Line)
            {
                return string.Create(formatProvider, $"({Start.Line}, {Start.Column}-{End.Column})");
            }
            else
            {
                return Start.Column == 0 && End.Column == 0
                     ? string.Create(formatProvider, $"({Start.Line}-{End.Line})")
                     : string.Create(formatProvider, $"({Start.Line}, {Start.Column}, {End.Line}, {End.Column})");
            }
        }

        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Place holder for future work" )]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1108:BlockStatementsMustNotContainEmbeddedComments", Justification = "Reviewed.")]
        private string FormatRuntime(IFormatProvider formatProvider)
        {
            if(OperatingSystem.IsWindows())
            {
                return FormatMsBuild(formatProvider);
            }
            else // TODO: Adjust this to format based on styles of additional runtimes
            {
                // for now - always use MSBUILD format
                return FormatMsBuild(formatProvider);
            }
        }
    }
}
