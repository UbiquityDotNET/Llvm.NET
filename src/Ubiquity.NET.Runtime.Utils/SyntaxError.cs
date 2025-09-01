// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Ubiquity.NET.TextUX;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Enumeration to indicate the source of an error during parse</summary>
    public enum ParseErrorSource
    {
        /// <summary>Error is a simple lexical error from the lexer</summary>
        Lexer,

        /// <summary>Error is from the parsing stage</summary>
        Parser,
    }

    /// <summary>Parse technology independent abstraction of a syntax error from the lexer or parser</summary>
    /// <typeparam name="T">Type of the enum for diagnostic IDs</typeparam>
    public class SyntaxError<T>
        where T : struct, Enum
    {
        /// <summary>Initializes a new instance of the <see cref="SyntaxError{T}"/> class</summary>
        /// <param name="source">Source of the error</param>
        /// <param name="sourceFile">Source file this error was found in</param>
        /// <param name="id">ID of the error</param>
        /// <param name="symbol">symbol the error is from</param>
        /// <param name="location">Location in <paramref name="sourceFile"/> the error was found</param>
        /// <param name="message">message for the error</param>
        /// <param name="exception">Any exception associated with the error</param>
        public SyntaxError(
            ParseErrorSource source,
            string sourceFile,
            T id,
            string symbol,
            SourceRange location,
            string message,
            Exception? exception
            )
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

        /// <summary>Gets the production source of the error</summary>
        public ParseErrorSource Source { get; }

        /// <summary>Gets the sourceFile containing the error</summary>
        public string SourceFile { get; }

        /// <summary>Gets the symbol related to the error</summary>
        public string Symbol { get; }

        /// <summary>Gets the ID of the error</summary>
        public T Id { get; }

        /// <summary>Gets the source location of the error in <see cref="SourceFile"/></summary>
        public SourceRange Location { get; }

        /// <summary>Gets the message for this error</summary>
        public string Message { get; }

        /// <summary>Gets any exceptions associated with this error</summary>
        public Exception? Exception { get; }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"{SourceFile}({Location}): error: {Source}{Id} {Message}";
        }
    }
}
