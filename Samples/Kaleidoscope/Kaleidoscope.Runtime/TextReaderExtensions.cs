// -----------------------------------------------------------------------
// <copyright file="TextReaderExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Kaleidoscope.Runtime
{
    /// <summary>Utility class to provide extensions for REPL Loop</summary>
    public static class TextReaderExtensions
    {
        [SuppressMessage( "StyleCop.CSharp.LayoutRules", "SA1500:Braces for multi-line statements should not share line", Justification = "Do/While Loop" )]
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

        /// <summary>Async operator to encapsulate conversion of text from a <see cref="TextReader"/> into an observable sequence of Kaleidoscope statements</summary>
        /// <param name="reader">Input reader</param>
        /// <param name="prompt">Action to provide prompts when the transform requires new data from the reader</param>
        /// <returns>Observable sequence of complete statements ready for parsing</returns>
        public static async IAsyncEnumerable<string> ToStatements(
            this TextReader reader,
            Action<ReadyState>? prompt,
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

                var partials = SplitLines(bldr, line, cancelToken);
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
            CancellationToken cancelToken = default
            )
        {
            string[ ] statements = line.Split( ';' );

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
                buffer.Append( ';' );
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
