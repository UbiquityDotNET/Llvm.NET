// <copyright file="FormattedConsoleErrorListener.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Antlr4.Runtime;
using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar
{
    /// <summary>Implements an error strategy to write colored errors to the console</summary>
    /// <remarks>Errors are formatted to include a "^" to indicate the specific point in the input that caused the error</remarks>
    public class FormattedConsoleErrorListener
        : IUnifiedErrorListener
    {
        /// <inheritdoc/>
        public void SyntaxError( IRecognizer recognizer
                               , int offendingSymbol
                               , int line
                               , int charPositionInLine
                               , string msg
                               , RecognitionException e
                               )
        {
            recognizer.ValidateNotNull( nameof( recognizer ) );
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                Console.WriteLine( recognizer.InputStream.ToString( ).TrimEnd( ) );
                Console.WriteLine( new string( ' ', charPositionInLine ) + "^" );
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine( "error KLX{0:D04}: {1}"
                                        , recognizer.State
                                        , msg
                                        );
            }
            finally
            {
                Console.ResetColor( );
            }
        }

        /// <inheritdoc/>
        public void SyntaxError( IRecognizer recognizer
                               , IToken offendingSymbol
                               , int line
                               , int charPositionInLine
                               , string msg
                               , RecognitionException e
                               )
        {
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                string srcText = ( ( ITokenStream )recognizer.InputStream ).TokenSource.InputStream.ToString( );
                Console.WriteLine( srcText.TrimEnd() );
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( new string( ' ', charPositionInLine ) + '^' );
                Console.Error.WriteLine( "error KLP{0:D04}: {1}"
                                       , recognizer.State
                                       , msg
                                       );
            }
            finally
            {
                Console.ResetColor( );
            }
        }
    }
}
