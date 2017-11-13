// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;

[assembly: CLSCompliant( false )]
[assembly: SuppressMessage( "Microsoft.Naming", "CA1715", Justification = "Generated code", MessageId = "Result", Scope ="type", Target = "Kaleidoscope.Grammar.KaleidoscopeBaseVisitor`1" )]
[assembly: SuppressMessage( "Microsoft.Naming", "CA1715", Justification = "Generated code", MessageId = "Result", Scope ="type", Target = "Kaleidoscope.Grammar.IKaleidoscopeVisitor`1" )]
[assembly: SuppressMessage( "Microsoft.Naming", "CA1708", Justification = "Generated code")]

namespace Kaleidoscope.Grammar
{
    public enum LanguageLevel
    {
        /// <summary>Chapters 2-4</summary>
        SimpleExpressions,

        /// <summary>Chapter 5</summary>
        ControlFlow,

        /// <summary>Chapter 6</summary>
        UserDefinedOperators,

        /// <summary>Chapter 7</summary>
        MutableVariables,
    }

    // partial class customization to the ANTLR generated class
    public partial class KaleidoscopeParser
        : Parser
    {
        /// <summary>Gets or sets the Language level the application supports</summary>
        public LanguageLevel LanguageLevel { get; set; }

        /*
        These FeatureXXX properties are used in Semantic predicates in the grammar
        to control support of various language features.
        */

        /// <summary>Gets a value indicating whether the control flow language features are enabled</summary>
        public bool FeatureControlFlow => IsFeatureEnabled( LanguageLevel.ControlFlow );

        /// <summary>Gets a value indicating whethermutable variables are enabled</summary>
        public bool FeatureMutableVars => IsFeatureEnabled( LanguageLevel.MutableVariables );

        /// <summary>Gets a value indicating whether the control flow language features are enabled</summary>
        public bool FeatureUserOperators => IsFeatureEnabled( LanguageLevel.UserDefinedOperators );

        /// <inheritdoc/>
        public override void EnterRecursionRule( ParserRuleContext localctx, int state, int ruleIndex, int precedence )
        {
            // remap the ANLTR generated fixed precedence for any value > 0.
            // this effectively overrides the hard coded value since the grammar
            // can't specify ahead of time what the user might declare.
            if( precedence > 0 )
            {
                precedence = GetNextPrecedence( _input.Lt( -1 ) );
            }

            base.EnterRecursionRule( localctx, state, ruleIndex, precedence );
        }

        /// <inheritdoc/>
        public override bool Precpred( RuleContext localctx, int precedence )
        {
            // ignore passed in precedence. Instead compute it from the
            // following token as the grammar hard codes the precedence
            // value for each level in the grammar, but it can't know
            // that the user can add more dynamically in this language.
            var operatorInfo = GetPrecedence( _input.Lt( 1 ) );
            return base.Precpred( localctx, operatorInfo.Precedence );
        }

        /// <summary>Attempts to add a new user defined operator</summary>
        /// <param name="token">Symbol for the operator</param>
        /// <param name="kind"><see cref="OperatorKind"/> value to define the behavior of the operator</param>
        /// <param name="precedence">precedence level for the operator</param>
        /// <returns><see langword="true"/> if the operator was added and <see langword="false"/> if not</returns>
        /// <remarks>
        /// This can add or replace user defined operators, howeve attempts to replace a built-in operator
        /// will not replace the operator and will simply return <see langword="false"/>.
        /// </remarks>
        public bool TryAddOperator( char token, OperatorKind kind, int precedence )
            => BinOpPrecedence.TryAddOrReplaceItem( new OperatorInfo( token, kind, precedence, false ) );

        private bool IsPrefixOp( IToken op )
        {
            var operatorInfo = GetPrecedence( op );
            return operatorInfo.Kind == OperatorKind.PreFix;
        }

        private bool IsFeatureEnabled( LanguageLevel feature )
        {
            return LanguageLevel >= feature;
        }

        private OperatorInfo GetPrecedence( IToken op )
        {
            if( BinOpPrecedence.TryGetValue( op.Text[0], out var value ) )
            {
                return value;
            }

            return default;
        }

        private int GetNextPrecedence( IToken op )
        {
            var operatorInfo = GetPrecedence( op );
            if( operatorInfo.Kind == OperatorKind.InfixRightAssociative || operatorInfo.Kind == OperatorKind.PreFix )
            {
                return operatorInfo.Precedence;
            }

            return operatorInfo.Precedence + 1;
        }

        private OperatorInfoCollection BinOpPrecedence = new OperatorInfoCollection()
        {
            new OperatorInfo( '<', OperatorKind.InfixLeftAssociative, 10, true),
            new OperatorInfo( '>', OperatorKind.InfixLeftAssociative, 10, true),
            new OperatorInfo( '+', OperatorKind.InfixLeftAssociative, 20, true),
            new OperatorInfo( '-', OperatorKind.InfixLeftAssociative, 20, true),
            new OperatorInfo( '*', OperatorKind.InfixLeftAssociative, 40, true),
            new OperatorInfo( '/', OperatorKind.InfixLeftAssociative, 40, true),
            new OperatorInfo( '^', OperatorKind.InfixRightAssociative, 50, true),
        };
    }
}
