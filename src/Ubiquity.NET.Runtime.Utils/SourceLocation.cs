// -----------------------------------------------------------------------
// <copyright file="SourceLocation.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Abstraction to hold a source location</summary>
    /// <param name="StartLine">Line number of the starting line [1..n][0 = uninitialized/unknown]</param>
    /// <param name="StartColumn">Starting column position of the location [0..n-1]</param>
    /// <param name="EndLine">Ending line number of the location [1..n]</param>
    /// <param name="EndColumn">Ending column position of the location [0..n-1]</param>
    public readonly record struct SourceLocation( int StartLine, int StartColumn, int EndLine, int EndColumn )
    {
        /// <inheritdoc/>
        public override string ToString( )
        {
            return StartLine == EndLine && StartColumn == EndColumn
                ? $"({StartLine},{StartColumn})"
                : $"({StartLine},{StartColumn},{EndLine},{EndColumn})";
        }
    }
}
