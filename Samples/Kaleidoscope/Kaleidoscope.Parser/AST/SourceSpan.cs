// <copyright file="SourceSpan.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

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

        public override bool Equals( object obj )
        {
            return obj is SourceSpan
                && Equals( ( SourceSpan )obj );
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
            return $"({StartLine},{StartColumn})-({EndLine},{EndColumn})";
        }

        public override int GetHashCode( )
        {
            int hashCode = 2078777074;
            hashCode = (hashCode * -1521134295) + base.GetHashCode( );
            hashCode = (hashCode * -1521134295) + StartLine.GetHashCode( );
            hashCode = (hashCode * -1521134295) + StartColumn.GetHashCode( );
            hashCode = (hashCode * -1521134295) + EndLine.GetHashCode( );
            hashCode = (hashCode * -1521134295) + EndColumn.GetHashCode( );
            return hashCode;
        }

        public static bool operator ==( SourceSpan span1, SourceSpan span2 ) => span1.Equals( span2 );

        public static bool operator !=( SourceSpan span1, SourceSpan span2 ) => !( span1 == span2 );
    }
}
