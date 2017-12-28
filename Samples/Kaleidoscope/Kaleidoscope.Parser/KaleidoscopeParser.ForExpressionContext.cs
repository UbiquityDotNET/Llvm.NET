// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class ForExpressionContext
        {
            public InitializerContext Initializer => initializer( );

            public IReadOnlyList<ExpressionContext> Expressions => expression( );

            public ExpressionContext EndExpression => expression( 0 );

            public ExpressionContext StepExpression => Expressions.Count > 2 ? expression( 1 ) : null;

            public ExpressionContext BodyExpression => expression( Expressions.Count - 1 );
        }
    }
}
