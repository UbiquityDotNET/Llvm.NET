// <copyright file="ReplLoopExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kaleidoscope.Runtime
{
    /// <summary>Utility class to provide extensions for REPL Loop</summary>
    public static class TextReaderExtensions
    {
        /// <summary>Gets an enumerable that pulls lines of text from a <see cref="TextReader"/></summary>
        /// <param name="reader">Reader to read from</param>
        /// <returns>Enumerable of the lines from the reader</returns>
        /// <remarks>
        /// <para>This lazy pulls the lines from the reader and, depending on the source the
        /// reader represents it may be an infinite stream. Callers, must not assume the
        /// sequence has a natural end without some knowledge of the source. If the source
        /// does have an end then the enumeration will complete when that end is reached.
        /// </para>
        /// <para>This is useful in REPL style interactive parsing to pull lines of text from
        /// the console. In such a case the number of lines is infinite but the REPL loop that
        /// pulls the lines can still choose when to stop.</para>
        /// </remarks>
        public static IEnumerable<string> ReadLines( this TextReader reader )
        {
            while( true )
            {
                string line = reader.ReadLine( );
                if( line == null )
                {
                    yield break;
                }

                yield return line;
            }
        }

        /// <summary>Reads lines and statements from a text reader</summary>
        /// <param name="reader">Source to read text from</param>
        /// <returns>Enumerable set of lines that may be partial statements</returns>
        /// <remarks>
        /// Each value enumerated includes the full text since the last complete statement.
        /// That is the text from multiple partial statements will include the text of the
        /// preceding partial. Once a complete statement is found the entire string is provided
        /// with the IsPartial property set to <see langword="false"/>
        /// </remarks>
        public static IEnumerable<(string Txt, bool IsPartial)> ReadStatements( this TextReader reader )
        {
            return reader.ReadLines( ).ReadStatements( );
        }

        /// <summary>Reads lines and statements from a source of lines</summary>
        /// <param name="lines">Source to pull lines of text from</param>
        /// <returns>Enumerable set of lines that may be partial statements</returns>
        /// <remarks>
        /// Each value enumerated includes the full text since the last complete statement.
        /// That is the text from multiple partial statements will include the text of the
        /// preceding partial. Once a complete statement is found the entire string is provided
        /// with the IsPartial property set to <see langword="false"/>
        /// </remarks>
        public static IEnumerable<(string Txt, bool IsPartial)> ReadStatements( this IEnumerable<string> lines )
        {
            var bldr = new StringBuilder( );
            foreach( string line in lines )
            {
                string[ ] statements = line.Split( ';' );

                // if the last line in the group was terminated with a ; the
                // the last entry is an empty string, but a single blank line
                // as input isn't considered completed.
                int completeStatements = statements.Length - 1;
                bool wasLastTerminated = statements[ statements.Length - 1 ] == string.Empty && statements.Length > 1;
                if( wasLastTerminated && completeStatements > 1 )
                {
                    ++completeStatements;
                }

                for( int i =0; i< completeStatements; ++i )
                {
                    string statement = statements[ i ];
                    bldr.Append( statement );
                    bldr.Append( ';' );
                    bldr.AppendLine( );

                    yield return (bldr.ToString( ), false );

                    if( bldr.Length > statement.Length + 1 )
                    {
                        bldr.Clear( );
                    }
                }

                if( !wasLastTerminated )
                {
                    string partial = statements[ statements.Length - 1 ];
                    bldr.AppendLine( partial );
                    yield return ( partial, true );
                }
            }
        }
    }
}
