// -----------------------------------------------------------------------
// <copyright file="AntlrUtilities.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using static System.Math;

namespace Kaleidoscope.Grammar
{
    /// <summary>Utility functions for extending ANTLR types</summary>
    public static class AntlrUtilities
    {
        /// <summary>Gets a character based interval from a <see cref="ParserRuleContext"/></summary>
        /// <param name="ruleContext">context to get the interval from</param>
        /// <returns>Character based interval covered by the context</returns>
        public static Interval GetCharInterval( this ParserRuleContext ruleContext )
        {
            if( ruleContext.start.Type == Recognizer<IToken, Antlr4.Runtime.Atn.ParserATNSimulator>.Eof )
            {
                return Interval.Invalid;
            }

            int startChar = ruleContext.Start.StartIndex;
            int endChar = ruleContext.Stop.StopIndex - 1;
            return Interval.Of( Min( startChar, endChar ), Max( startChar, endChar ) );
        }

        /// <summary>Gets the source <see cref="ICharStream"/> from a recognizer if it is available</summary>
        /// <param name="recognizer">Recognizer to get the stream from</param>
        /// <returns>The character stream or null if not available.</returns>
        public static ICharStream GetSourceStream( this IRecognizer recognizer )
        {
            if( recognizer.InputStream != null && recognizer.InputStream is ITokenStream tokenStream )
            {
                return tokenStream.TokenSource.InputStream;
            }

            return null;
        }

        /// <summary>Gets the source input text for a <see cref="ParserRuleContext"/> produced by an <see cref="IRecognizer"/></summary>
        /// <param name="ruleContext">Rule context to get the source text from</param>
        /// <param name="recognizer">Recognizer that produced <paramref name="ruleContext"/></param>
        /// <returns>Source contents for the rule or an empty string if the source is not available</returns>
        public static string GetSourceText( this ParserRuleContext ruleContext, IRecognizer recognizer )
        {
            return GetSourceText( ruleContext, GetSourceStream( recognizer ) );
        }

        /// <summary>Gets the source input text for a <see cref="ParserRuleContext"/> produced by an <see cref="IRecognizer"/></summary>
        /// <param name="ruleContext">Rule context to get the source text from</param>
        /// <param name="charStream">The stream the rule was parsed from</param>
        /// <returns>Source contents for the rule or an empty string if the source is not available</returns>
        public static string GetSourceText( this ParserRuleContext ruleContext, ICharStream charStream )
        {
            if( charStream == null )
            {
                return string.Empty;
            }

            var span = ruleContext.GetCharInterval( );
            if( span.a < 0 )
            {
                return string.Empty;
            }

            return charStream.GetText( span );
        }

        /// <summary>Generates a Unique ID for a parse tree node</summary>
        /// <param name="tree">parse tree to generate the id for</param>
        /// <returns>ID for the node</returns>
        /// <remarks>
        /// This is useful when generating various graph visualization file formats as
        /// they normally need unique IDs for each node to maintain proper references
        /// between the nodes.
        /// </remarks>
        public static string GetUniqueNodeId( this IParseTree tree )
        {
            var bldr = new StringBuilder( tree.GetHashCode( ).ToString( CultureInfo.InvariantCulture ) );
            if( tree.Parent != null )
            {
                bldr.Append( tree.Parent.GetChildIndex( tree ) );
                bldr.Append( tree.Parent.GetUniqueNodeId( ) );
            }

            return bldr.ToString( );
        }

        /// <summary>Determines the index of a child item in the parent</summary>
        /// <param name="tree">Parent tree to find the item in</param>
        /// <param name="item">Item to determine the index of</param>
        /// <returns>Zero based index in the parent or -1 if the item is not a child of <paramref name="tree"/></returns>
        public static int GetChildIndex( this IParseTree tree, IParseTree item )
        {
            for( int i = 0; i < tree.ChildCount; ++i )
            {
                if( item == tree.GetChild( i ) )
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>Enables enumeration of characters from a <see cref="StringBuilder"/></summary>
        /// <param name="bldr">Builder to enumerate</param>
        /// <returns>Enumerable for all the characters in the builder</returns>
        public static IEnumerable<char> AsEnumerable( this StringBuilder bldr )
        {
            for( int i = 0; i < bldr.Length; ++i )
            {
                yield return bldr[ i ];
            }
        }
    }
}
