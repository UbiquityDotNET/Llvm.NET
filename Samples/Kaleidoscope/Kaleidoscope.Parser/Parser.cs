// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    // This extends the individual parse tree context node types
    // so that labels in the grammar are unnecessary.
    public partial class KaleidoscopeParser
        : Parser
    {
        public partial class IdentifierContext
        {
            public string Name => Identifier( ).ToString( );
        }

        public partial class VariableExpressionContext
        {
            public string Name => ( ( IdentifierContext )GetChild( 0 ) ).Name;
        }

        public partial class ConstExpressionContext
        {
            public double Value => double.Parse( Number( ).GetText( ) );
        }

        public partial class ParenExpressionContext
        {
            public ExpressionContext Expression => ( ExpressionContext )GetChild( 1 );
        }

        public partial class ExternalDeclarationContext
        {
            public PrototypeContext Signature => prototype( );
        }

        public partial class FunctionCallExpressionContext
        {
            public string CaleeName => ( ( IdentifierContext )GetChild( 0 ) ).Name;

            public IReadOnlyList<ExpressionContext> Args => expression( );
        }

        public partial class InitializerContext
        {
            public string Name => identifier( ).Name;

            public ExpressionContext Value => expression( );
        }

        public partial class VarInExpressionContext
        {
            public IReadOnlyList<InitializerContext> Initiaizers => initializer( );

            public ExpressionContext Scope => GetRuleContext<ExpressionContext>( ChildCount - 1 );
        }

        public partial class ConditionalExpressionContext
        {
            public ExpressionContext Condition => expression( 0 );

            public ExpressionContext ThenExpression => expression( 1 );

            public ExpressionContext ElseExpression => expression( 2 );
        }

        public partial class ForExpressionContext
        {
            public InitializerContext Initializer => initializer( );

            public ExpressionContext EndExpression => expression( 0 );

            public ExpressionContext StepExpression => expression( 1 );

            public ExpressionContext BodyExpression => expression( 2 );
        }

        public partial class AssignmentExpressionContext
        {
            public string VariableName => identifier( ).Name;

            public ExpressionContext Value => expression( );
        }

        public partial class UnaryOpExpressionContext
        {
            public char Op => opsymbol( ).GetText( )[ 0 ];

            public ExpressionContext Rhs => expression( );

            public IEnumerable<ExpressionContext> Args
            {
                get
                {
                    yield return Rhs;
                }
            }

            public OperatorKind GetOperatorInfo( KaleidoscopeParser parser )
            {
                if( parser.UnaryOps.TryGetValue( Op, out OperatorInfo entry ) )
                {
                    return entry.Kind;
                }

                return OperatorKind.None;
            }
        }

        public partial class BinaryOpExpressionContext
        {
            public ExpressionContext Lhs => expression( 0 );

            public char Op => opsymbol( ).GetText( )[ 0 ];

            public ExpressionContext Rhs => expression( 1 );

            public IEnumerable<ExpressionContext> Args => expression( );

            public OperatorKind GetOperatorInfo( KaleidoscopeParser parser )
            {
                if( parser.BinOpPrecedence.TryGetValue( Op, out OperatorInfo entry ) )
                {
                    return entry.Kind;
                }

                return OperatorKind.None;
            }
        }

        public partial class PrototypeContext
        {
            public virtual IReadOnlyList<string> Parameters => new List<string>( );
        }

        public partial class UnaryProtoTypeContext
        {
            public char Op => opsymbol( ).GetText( )[ 0 ];

            public override IReadOnlyList<string> Parameters => new List<string> { identifier( ).GetText( ) };
        }

        public partial class BinaryProtoTypeContext
        {
            public char Op => opsymbol( ).GetText( )[ 0 ];

            public override IReadOnlyList<string> Parameters => identifier( ).Select( i => i.Name ).ToList( );

            public int Precedence => ( int )double.Parse( Number( ).GetText() );
        }

        public partial class FunctionProtoTypeContext
        {
            public string Name => identifier( 0 ).Name;

            public override IReadOnlyList<string> Parameters => identifier( ).Skip( 1 ).Select( i => i.Name ).ToList( );
        }

        public partial class FunctionDefinitionContext
        {
            public PrototypeContext Signature => prototype( );

            public ExpressionContext BodyExpression => expression( );
        }

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
        {
            switch( kind )
            {
            case OperatorKind.InfixLeftAssociative:
            case OperatorKind.InfixRightAssociative:
                return BinOpPrecedence.TryAddOrReplaceItem( new OperatorInfo( token, kind, precedence, false ) );

            case OperatorKind.PreFix:
                return UnaryOps.TryAddOrReplaceItem( new OperatorInfo( token, kind, 0, false ) );

            // case OperatorKind.None:
            default:
                throw new ArgumentException( "unknown kind", nameof( kind ) );
            }
        }

        public virtual bool IsPrefixOp( IToken op )
        {
            return UnaryOps.TryGetValue( op.Text[0], out var value );
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

        private OperatorInfoCollection UnaryOps = new OperatorInfoCollection();

        private OperatorInfoCollection BinOpPrecedence = new OperatorInfoCollection()
        {
            new OperatorInfo( '<', OperatorKind.InfixLeftAssociative, 10, true),
            new OperatorInfo( '+', OperatorKind.InfixLeftAssociative, 20, true),
            new OperatorInfo( '-', OperatorKind.InfixLeftAssociative, 20, true),
            new OperatorInfo( '*', OperatorKind.InfixLeftAssociative, 40, true),
            new OperatorInfo( '/', OperatorKind.InfixLeftAssociative, 40, true),
            new OperatorInfo( '^', OperatorKind.InfixRightAssociative, 50, true),
        };
    }
}
