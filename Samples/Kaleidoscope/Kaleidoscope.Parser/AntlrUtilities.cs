// <copyright file="AntlrUtilities.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

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
            int startChar = ruleContext.Start.StartIndex;
            int endChar = ruleContext.Stop.StopIndex;
            return new Interval( Math.Min( startChar, endChar), Math.Max( startChar, endChar ) );
        }

        public static string GetUniqueNodeId( this IParseTree tree )
        {
            var bldr = new StringBuilder( tree.GetHashCode( ).ToString( ) );
            if( tree.Parent != null )
            {
                bldr.Append( tree.Parent.GetChildIndex( tree ) );
                bldr.Append( tree.Parent.GetUniqueNodeId( ) );
            }

            return bldr.ToString( );
        }

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

        public static IEnumerable<char> AsEnumerable( this StringBuilder bldr )
        {
            for( int i = 0; i < bldr.Length; ++i )
            {
                yield return bldr[ i ];
            }
        }
    }
}
