// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Extensions
{
    /// <summary>Represents a single point position in a source input</summary>
    /// <remarks>
    /// Positions use a Line numbering similar to most editors that is 1 based.([1..n][0 = uninitialized/unknown]).
    /// Column position, also like most editors, use a 0 based value ([0..n-1]).
    /// The index, if available, indicates the 0 based index into a theoretical vector of characters
    /// for the position. Not all location information includes such a thing so it may not have a value.
    /// </remarks>
    [DebuggerDisplay("{DebuggerToString(),nq}")]
    public readonly record struct SourcePosition
    {
        /// <summary>Initializes a new instance of the <see cref="SourcePosition"/> struct.</summary>
        /// <param name="line">1 based line number of this position ([1..n][0 = uninitialized/unknown])</param>
        /// <param name="column">0 based column position of this position ([0..n-1])</param>
        /// <param name="index">0 based index of this position (Optional)</param>
        [SetsRequiredMembers]
        public SourcePosition(int line, int column, int? index = default)
        {
            Line = line;
            Column = column;
            Index = index;
        }

        /// <summary>Gets the 0 based index from the start of the source to this position if available</summary>
        public int? Index { get; init; }

        /// <summary>Gets the 0 based column value of this position</summary>
        public required int Column
        {
            get;
            init
            {
#if NET7_0_OR_GREATER
                ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
#else
                if(value.CompareTo(0) < 0)
                {
                    throw new ArgumentOutOfRangeException(SR.Format(nameof(Resources.ArgumentOutOfRange_Generic_MustBeGreaterOrEqual), nameof(value), value, 0));
                }
#endif
                field = value;
            }
        }

        /// <summary>Gets the one based line position of the location</summary>
        public required int Line
        {
            get;
            init
            {
#if NET7_0_OR_GREATER
                ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 0);
#else
                if(value.CompareTo(0) <= 0)
                {
                    throw new ArgumentOutOfRangeException(SR.Format(nameof(Resources.ArgumentOutOfRange_Generic_MustBeGreater), nameof(value), value, 0));
                }
#endif
                field = value;
            }
        }

        /// <summary>Produces a string form of this position</summary>
        /// <returns>string form of the position</returns>
        public override string ToString( )
        {
            return $"({Line},{Column})";
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string DebuggerToString()
        {
            return $"({Line},{Column}); [{Index}]";
        }
    }
}
