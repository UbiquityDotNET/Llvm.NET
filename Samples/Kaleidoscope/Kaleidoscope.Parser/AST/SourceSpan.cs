// -----------------------------------------------------------------------
// <copyright file="SourceSpan.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Kaleidoscope.Grammar
{
    public struct SourceSpan
        : IEquatable<SourceSpan>
    {
        public SourceSpan( int startLine, int startColumn, int endLine, int endColumn )
        {
            StartLine = startLine;
            StartColumn = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
        }

        public int StartLine { get; }

        public int StartColumn { get; }

        public int EndLine { get; }

        public int EndColumn { get; }

        public override bool Equals( object? obj )
        {
            return obj is SourceSpan span
                && Equals( span );
        }

        public bool Equals( SourceSpan other )
        {
            return StartLine == other.StartLine
                && StartColumn == other.StartColumn
                && EndLine == other.EndLine
                && EndColumn == other.EndColumn;
        }

        public override string ToString( )
        {
            return StartLine == EndLine && StartColumn == EndColumn
                ? $"({StartLine},{StartColumn})"
                : $"({StartLine},{StartColumn},{EndLine},{EndColumn})";
        }

        public override int GetHashCode( )
        {
            return HashCode.Combine( StartLine, StartColumn, EndLine, EndColumn );
        }

        public static bool operator ==( SourceSpan span1, SourceSpan span2 ) => span1.Equals( span2 );

        public static bool operator !=( SourceSpan span1, SourceSpan span2 ) => !( span1 == span2 );
    }
}
