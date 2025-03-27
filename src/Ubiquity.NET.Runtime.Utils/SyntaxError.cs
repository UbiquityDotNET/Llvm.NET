// -----------------------------------------------------------------------
// <copyright file="SyntaxError.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.Runtime.Utils
{
    public enum ParseErrorSource
    {
        Lexer,
        Parser,
    }

    public class SyntaxError
    {
        public SyntaxError( ParseErrorSource source, string sourceFile, int id, string symbol, SourceSpan location, string message, Exception? exception )
        {
            ArgumentNullException.ThrowIfNull( sourceFile );
            ArgumentNullException.ThrowIfNull( symbol );
            ArgumentNullException.ThrowIfNull( message );

            Source = source;
            SourceFile = sourceFile;
            Id = id;
            Symbol = symbol;
            Location = location;
            Message = message;
            Exception = exception;
        }

        public ParseErrorSource Source { get; }

        public string SourceFile { get; }

        public string Symbol { get; }

        public int Id { get; }

        public SourceSpan Location { get; }

        public string Message { get; }

        public Exception? Exception { get; }

        public override string ToString( )
        {
            return $"{SourceFile}({Location}): error: {Source}{Id:D04} {Message}";
        }
    }
}
