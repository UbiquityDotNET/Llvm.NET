// <copyright file="AntlrUtilities.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

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
            return new Interval( startChar, endChar );
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
