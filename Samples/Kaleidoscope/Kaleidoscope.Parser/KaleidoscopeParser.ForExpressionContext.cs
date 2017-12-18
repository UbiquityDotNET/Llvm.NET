// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class ForExpressionContext
        {
            public InitializerContext Initializer => initializer( );

            public ExpressionContext EndExpression => expression( 0 );

            public ExpressionContext StepExpression => expression( 1 );

            public ExpressionContext BodyExpression => expression( 2 );
        }
    }
}
