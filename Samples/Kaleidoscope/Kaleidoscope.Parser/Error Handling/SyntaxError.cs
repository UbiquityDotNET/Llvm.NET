// -----------------------------------------------------------------------
// <copyright file="SyntaxError.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar
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
            Source = source;
            SourceFile = sourceFile.ValidateNotNull( nameof( sourceFile ) );
            Id = id;
            Symbol = symbol.ValidateNotNull( nameof( symbol ) );
            Location = location;
            Message = message.ValidateNotNull( nameof( message ) );
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
