// -----------------------------------------------------------------------
// <copyright file="TextReaderExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Text;

namespace Kaleidoscope.Runtime
{
    /// <summary>Utility class to provide extensions for REPL Loop</summary>
    public static class TextReaderExtensions
    {
        /// <summary>Transforms a <see cref="TextReader"/> to an observable sequence of lines</summary>
        /// <param name="input">Input reader</param>
        /// <param name="prompt"><see cref="Action"/> to provide a prompt before new lines are read from the reader</param>
        /// <returns>Observable of the lines read from the reader</returns>
        public static IObservable<string> ToObservableLines( this TextReader input, Action? prompt )
        {
            return Observable.Create<string>( async observer =>
            {
                string line;
                do
                {
                    prompt?.Invoke( );

                    line = await input.ReadLineAsync( ).ConfigureAwait(true);
                    if( line != null )
                    {
                        observer.OnNext( line );
                    }
                    else
                    {
                        observer.OnCompleted( );
                    }
                }
                while( line != null );
            } );
        }

        /// <summary>Transforms a sequence of lines into a sequence of statements (Separated by a ";")</summary>
        /// <param name="lines">Sequence of lines to transform</param>
        /// <returns>Observable sequence of lines, marked as partials or full statements</returns>
        public static IObservable<(string Txt, bool IsPartial)> AsStatements( this IObservable<string> lines )
        {
            var bldr = new StringBuilder( );
            return from line in lines
                   from partial in SplitLines( bldr, line )
                   select partial;
        }

        /// <summary>Transforms a sequence of potentially partial statements into a sequence of complete statements</summary>
        /// <param name="partials">Sequence of potentially partial lines</param>
        /// <returns>Observable of full statements</returns>
        public static IObservable<string> AsFullStatements( this IObservable<(string Txt, bool IsPartial)> partials)
        {
            return from p in partials
                   where !p.IsPartial
                   select p.Txt;
        }

        /// <summary>Rx.NET operator to encapsulate conversion of text from a <see cref="TextReader"/> into an observable sequence of Kaleidoscope statements</summary>
        /// <param name="reader">Input reader</param>
        /// <param name="prompt">Action to provide prompts when the transform requires new data from the reader</param>
        /// <returns>Observable sequence of complete statements ready for parsing</returns>
        public static IObservable<string> ToObservableStatements(this TextReader reader, Action<ReadyState>? prompt )
        {
            var stateManager = new ReadyStateManager( );

            return reader.ToObservableLines( ( ) => prompt?.Invoke( stateManager.State ) )
                         .AsStatements( )
                         .Do( s => stateManager.UpdateState( s.Txt, s.IsPartial ) )
                         .AsFullStatements( );
        }

        private static IEnumerable<(string Txt, bool IsPartial)> SplitLines( StringBuilder buffer, string line )
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
