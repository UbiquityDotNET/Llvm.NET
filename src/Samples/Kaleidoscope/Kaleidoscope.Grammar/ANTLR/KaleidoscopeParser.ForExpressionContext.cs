// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class ForExpressionContext
        {
            public InitializerContext Initializer => initializer();

            public IReadOnlyList<ExpressionContext> Expressions => expression();

            public ExpressionContext EndExpression => expression( 0 );

            public ExpressionContext? StepExpression => Expressions.Count > 2 ? expression( 1 ) : null;

            public ExpressionContext BodyExpression => expression( Expressions.Count - 1 );
        }
    }
}
