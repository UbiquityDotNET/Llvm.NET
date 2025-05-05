// -----------------------------------------------------------------------
// <copyright file="TextReaderExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Utility class to provide extensions for REPL Loop and other scenarios requiring Async processing of text input</summary>
    public static class TextReaderExtensions
    {
        /// <summary>Provides an async sequence of lines from a reader</summary>
        /// <param name="input">reader to retrieve lines from</param>
        /// <param name="cancelToken">Cancelation token to cancel production of lines</param>
        /// <returns>Async sequence of lines from <paramref name="input"/></returns>
        public static async IAsyncEnumerable<string> ToLinesAsync( this TextReader input, [EnumeratorCancellation] CancellationToken cancelToken = default )
        {
            ArgumentNullException.ThrowIfNull( input );

            string? line;
            do
            {
                line = await input.ReadLineAsync(cancelToken);
                if( line != null )
                {
                    yield return line;
                }
            } while( line != null );
        }

        /// <inheritdoc cref="ToStatements(TextReader, Action{ReadyState}?, char, CancellationToken)"/>
        /// <remarks>
        /// This uses a common statement completion character of ';' if that is not desired then the
        /// <see cref="ToStatements(TextReader, Action{ReadyState}?, char, CancellationToken)"/> overload
        /// takes a parameter for the completion character.
        /// </remarks>
        public static IAsyncEnumerable<string> ToStatements(
            this TextReader reader,
            Action<ReadyState>? prompt,
            CancellationToken cancelToken = default
            )
        {
            return ToStatements(reader, prompt, ';', cancelToken);
        }

        /// <summary>Async operator to encapsulate conversion of text from a <see cref="TextReader"/> into an observable sequence of Kaleidoscope statements</summary>
        /// <param name="reader">Input reader</param>
        /// <param name="prompt">Action to provide prompts when the transform requires new data from the reader</param>
        /// <param name="terminationChar">Character to mark termination of the statement</param>
        /// <param name="cancelToken">Cancelation token for the async operation</param>
        /// <returns>Async sequence of complete statements ready for parsing</returns>
        public static async IAsyncEnumerable<string> ToStatements(
            this TextReader reader,
            Action<ReadyState>? prompt,
            char terminationChar,
            [EnumeratorCancellation] CancellationToken cancelToken = default
            )
        {
            var stateManager = new ReadyStateManager( prompt );
            var bldr = new StringBuilder( );
            await foreach( string line in reader.ToLinesAsync( cancelToken) )
            {
                if(cancelToken.IsCancellationRequested)
                {
                    break;
                }

                var partials = SplitLines(bldr, line, terminationChar, cancelToken);
                foreach( var (txt, isPartial) in partials )
                {
                    stateManager.UpdateState( txt, isPartial );
                    if( !isPartial )
                    {
                        yield return txt;
                    }
                }

                stateManager.Prompt( );
            }
        }

        private static IEnumerable<(string Txt, bool IsPartial)> SplitLines(
            StringBuilder buffer,
            string line,
            char terminationChar,
            CancellationToken cancelToken = default
            )
        {
            string[ ] statements = line.Split( terminationChar );

            // if the last line in the group was terminated with a ; the
            // the last entry is an empty string, but a single blank line
            // as input isn't considered completed.
            int completeStatements = statements.Length - 1;
            bool wasLastTerminated = string.IsNullOrEmpty( statements[ ^1 ] ) && statements.Length > 1;
            if( wasLastTerminated && completeStatements > 1 )
            {
                ++completeStatements;
            }

            for( int i = 0; i < completeStatements; ++i )
            {
                if (cancelToken.IsCancellationRequested)
                {
                    yield break;
                }

                string statement = statements[ i ];
                buffer.Append( statement );
                buffer.Append( terminationChar );
                buffer.AppendLine( );

                yield return (buffer.ToString( ), false);

                if( buffer.Length > statement.Length + 1 )
                {
                    buffer.Clear( );
                }
            }

            if( !wasLastTerminated )
            {
                string partial = statements[ ^1 ];
                buffer.AppendLine( partial );
                yield return (partial, true);
            }
        }
    }
}
