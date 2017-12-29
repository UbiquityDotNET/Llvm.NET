// <copyright file="ReplErrorStrategy.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;

namespace Kaleidoscope.Runtime
{
    /// <summary>Error handling strategy for REPL scenarios</summary>
    /// <remarks>
    /// This strategy handles creating more meaningful messages on feature predicate failures.
    /// Additionally, error recovery that results from reaching EOF on an otherwise succesful parse
    /// is translated into a <see cref="ParseCanceledException"/> to prevent any further attempts
    /// at recovery. This allows for detection of the incomplete statements when used in an interactive
    /// REPL application. In such a case the incomplete code could be combined with new text from the
    /// user until it either succeeds or fails due to a syntax error.
    /// </remarks>
    public class ReplErrorStrategy
        : DefaultErrorStrategy
    {
        protected override void ReportFailedPredicate( Parser recognizer, FailedPredicateException e )
        {
            string pred = e.Predicate;
            if( pred[0] == '!' )
            {
                pred = pred.Substring( 1 );
            }

            switch( e.Predicate )
            {
            case "FeatureControlFlow":
                NotifyErrorListeners( recognizer, "Control flow expressions not supported in this version of the language", e );
                break;

            case "FeatureConditionalExpr":
                NotifyErrorListeners( recognizer, "Conditional expressions not supported in this version of the language", e );
                break;

            case "FeatureMutableVars":
                NotifyErrorListeners( recognizer, "Mutable variables not supported in this version of the language", e );
                break;

            default:
                base.ReportFailedPredicate( recognizer, e );
                break;
            }
        }
    }
}
