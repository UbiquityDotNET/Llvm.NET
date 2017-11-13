// <copyright file="ReplErrorStrategy.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Kaleidoscope.Grammar
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
        /// <summary>Blocks recovery and throws an exception to prevent further processing</summary>
        /// <param name="recognizer">parser that encountered an error</param>
        /// <param name="e">exception describing the error</param>
        public override void Recover( Parser recognizer, RecognitionException e )
        {
            for( ParserRuleContext ruleCtx = recognizer.Context; ruleCtx != null; ruleCtx = ( ParserRuleContext )ruleCtx.Parent )
            {
                ruleCtx.exception = e;
            }

            throw new ParseCanceledException( e );
        }

        /// <summary>Blocks recovery and throws an exception to prevent further processing</summary>
        /// <param name="recognizer">parser that encountered an error</param>
        /// <returns>
        /// Nothing, this always throws an exception to prevent further parsing.
        /// </returns>
        public override IToken RecoverInline( Parser recognizer )
        {
            var ex = new InputMismatchException( recognizer );
            for( ParserRuleContext ruleCtx = recognizer.Context; ruleCtx != null; ruleCtx = ( ParserRuleContext )ruleCtx.Parent )
            {
                ruleCtx.exception = ex;
            }

            throw new ParseCanceledException( ex );
        }

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
