// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class UnaryOpExpressionContext
        {
            public IToken OpToken => unaryop( ).start;

            public int Op => OpToken.Type;

            public ExpressionContext Rhs => expression( );

            public IEnumerable<ExpressionContext> Args
            {
                get
                {
                    yield return Rhs;
                }
            }
        }
    }
}
