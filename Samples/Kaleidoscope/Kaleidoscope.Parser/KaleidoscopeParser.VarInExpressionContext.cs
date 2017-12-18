// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class VarInExpressionContext
        {
            public IReadOnlyList<InitializerContext> Initiaizers => initializer( );

            public ExpressionContext Scope => GetRuleContext<ExpressionContext>( ChildCount - 1 );
        }
    }
}
