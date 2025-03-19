// -----------------------------------------------------------------------
// <copyright file="SourceSpan.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar
{
    public readonly record struct SourceSpan( int StartLine, int StartColumn, int EndLine, int EndColumn )
    {
        public override string ToString( )
        {
            return StartLine == EndLine && StartColumn == EndColumn
                ? $"({StartLine},{StartColumn})"
                : $"({StartLine},{StartColumn},{EndLine},{EndColumn})";
        }
    }
}
