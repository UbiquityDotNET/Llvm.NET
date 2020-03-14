// -----------------------------------------------------------------------
// <copyright file="AstBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Antlr4.Runtime;

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

using static Kaleidoscope.Grammar.KaleidoscopeParser;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Parse tree Visitor to construct the AST from a parse tree</summary>
    public sealed class AstBuilder
        : KaleidoscopeBaseVisitor<IAstNode>
    {
        public AstBuilder( DynamicRuntimeState globalState )
        {
            RuntimeState = globalState;
        }

        public override IAstNode VisitParenExpression( ParenExpressionContext context )
        {
            return context.Expression.Accept( this );
        }

        public override IAstNode VisitConstExpression( ConstExpressionContext context )
        {
            return new ConstantExpression( context.GetSourceSpan( ), context.Value );
        }

        public override IAstNode VisitVariableExpression( VariableExpressionContext context )
        {
            string varName = context.Name;
            if( !NamedValues.TryGetValue( varName, out IVariableDeclaration? declaration ) )
            {
                throw new CodeGeneratorException( $"Unknown variable name: {varName}" );
            }

            return new VariableReferenceExpression( context.GetSourceSpan( ), declaration );
        }

        public override IAstNode VisitFunctionCallExpression( FunctionCallExpressionContext context )
        {
            Prototype function = FindCallTarget( context.CaleeName );

            var args = from expCtx in context.expression( )
                       select ( IExpression )expCtx.Accept( this );

            return new FunctionCallExpression( context.GetSourceSpan( ), function, args );
        }

        public override IAstNode VisitExpression( ExpressionContext context )
        {
            // Expression: PrimaryExpression (op expression)*
            // where the sub-expressions are in evaluation order
            var lhs = ( IExpression )context.Atom.Accept( this );

            foreach( var (op, rhs) in context.OperatorExpressions )
            {
                lhs = CreateBinaryOperatorNode( lhs, op, ( IExpression )rhs.Accept( this ) );
            }

            return lhs;
        }

        public override IAstNode VisitExternalDeclaration( ExternalDeclarationContext context )
        {
            var retVal = ( Prototype )context.Signature.Accept( this );
            RuntimeState.FunctionDeclarations.AddOrReplaceItem( retVal );
            return retVal;
        }

        public override IAstNode VisitFunctionDefinition( FunctionDefinitionContext context )
        {
            BeginFunctionDefinition( );

            var sig = ( Prototype )context.Signature.Accept( this );
            foreach( var param in sig.Parameters )
            {
                Push( param );
            }

            var body = ( IExpression )context.BodyExpression.Accept( this );

            var retVal = new FunctionDefinition( context.GetSourceSpan( )
                                               , sig
                                               , body
                                               , LocalVariables.ToImmutableArray( )
                                               );
            RuntimeState.FunctionDefinitions.AddOrReplaceItem( retVal );
            return retVal;
        }

        public override IAstNode VisitTopLevelExpression( TopLevelExpressionContext context )
        {
            BeginFunctionDefinition( );
            var sig = new Prototype( context.GetSourceSpan( ), RuntimeState.GenerateAnonymousName( ), true );
            var body = ( IExpression )context.expression( ).Accept( this );
            var retVal = new FunctionDefinition( context.GetSourceSpan( ), sig, body, true );
            RuntimeState.FunctionDefinitions.AddOrReplaceItem( retVal );
            return retVal;
        }

        #region UnaryOpExpression
        public override IAstNode VisitUnaryOpExpression( UnaryOpExpressionContext context )
        {
            // verify the operator was previously defined
            var opKind = RuntimeState.GetUnaryOperatorInfo( context.Op ).Kind;
            if( opKind == OperatorKind.None )
            {
                throw new CodeGeneratorException( $"invalid unary operator {context.Op}" );
            }

            string calleeName = CreateUnaryFunctionName( context.OpToken );
            var function = FindCallTarget( calleeName );
            if( function == null )
            {
                throw new CodeGeneratorException( $"Unknown function reference {calleeName}" );
            }

            var arg = ( IExpression )context.Rhs.Accept( this );
            return new FunctionCallExpression( context.GetSourceSpan( ), function, arg );
        }
        #endregion

        public override IAstNode VisitFullsrc( FullsrcContext context )
        {
            var children = from child in context.children
                           where !(child is TopLevelSemicolonContext)
                           select child.Accept( this );

            return new RootNode( context.GetSourceSpan( ), children );
        }

        public override IAstNode VisitRepl( ReplContext context )
        {
            var children = from child in context.children
                           select child.Accept( this );

            return new RootNode( context.GetSourceSpan( ), children );
        }

        public override IAstNode VisitConditionalExpression( ConditionalExpressionContext context )
        {
            var expressionSpan = context.GetSourceSpan( );

            // compiler generated result variable supports building conditional
            // expressions without the need for SSA form using mutable variables
            // The result is assigned a value from both sides of the branch. In
            // pure SSA form this isn't needed as a PHI node would be used instead.
            var retVal = new ConditionalExpression( expressionSpan
                                                  , ( IExpression )context.Condition.Accept( this )
                                                  , ( IExpression )context.ThenExpression.Accept( this )
                                                  , ( IExpression )context.ElseExpression.Accept( this )
                                                  , new LocalVariableDeclaration( expressionSpan, $"$ifresult${LocalVarIndex++}", null, true)
                                                  );
            Push( retVal.ResultVariable );
            return retVal;
        }

        public override IAstNode VisitForExpression( ForExpressionContext context )
        {
            var initializer = ( LocalVariableDeclaration )context.Initializer.Accept( this );

            ForInExpression retVal;
            using( NamedValues.EnterScope( ) )
            {
                Push( initializer );

                var step = ( IExpression )(context.StepExpression?.Accept( this ) ?? new ConstantExpression( default, 1.0 ));
                var body = ( IExpression )context.BodyExpression.Accept( this );

                var condition = ( IExpression )context.EndExpression.Accept( this );
                retVal = new ForInExpression( context.GetSourceSpan( ), initializer, condition, step, body );
            }

            return retVal;
        }

        public override IAstNode VisitVarInExpression( VarInExpressionContext context )
        {
            using( NamedValues.EnterScope( ) )
            {
                var localVariables = from initializer in context.Initiaizers
                                     select ( LocalVariableDeclaration )initializer.Accept( this );

                foreach( var local in localVariables )
                {
                    Push( local );
                }

                var body = ( IExpression )context.Scope.Accept( this );

                return new VarInExpression( context.GetSourceSpan( ), localVariables, body );
            }
        }

        public override IAstNode VisitPrototype( PrototypeContext context )
        {
            return BuildPrototype( context, context.Name );
        }

        public override IAstNode VisitFunctionPrototype( FunctionPrototypeContext context )
        {
            return BuildPrototype( context, context.Name );
        }

        public override IAstNode VisitInitializer( InitializerContext context )
        {
            var value = ( IExpression )(context.Value?.Accept( this ) ?? new ConstantExpression( context.GetSourceSpan( ), 0.0 ));
            return new LocalVariableDeclaration( context.GetSourceSpan( )
                                               , context.Name
                                               , value
                                               );
        }

        #region UserOperatorPrototypes
        public override IAstNode VisitBinaryPrototype( BinaryPrototypeContext context )
        {
            return BuildPrototype( context, CreateBinaryFunctionName( context.OpToken ) );
        }

        public override IAstNode VisitUnaryPrototype( UnaryPrototypeContext context )
        {
            return BuildPrototype( context, CreateUnaryFunctionName( context.OpToken ) );
        }
        #endregion

        [SuppressMessage( "Build", "CS8609:Nullability of reference types in return type doesn't match overridden member", Justification = "Override of method defined in assembly not using nullable types" )]
#pragma warning disable CS8609 // Nullability of reference types in return type doesn't match overridden member.
        protected override IAstNode? DefaultResult => null;
#pragma warning restore CS8609 // Nullability of reference types in return type doesn't match overridden member.

        private Prototype FindCallTarget( string calleeName )
        {
            // search defined functions first as they override extern declarations
            if( RuntimeState.FunctionDefinitions.TryGetValue( calleeName, out FunctionDefinition definition ) )
            {
                return definition.Signature;
            }

            // search extern declarations
            if( RuntimeState.FunctionDeclarations.TryGetValue( calleeName, out Prototype declaration ) )
            {
                return declaration;
            }

            throw new CodeGeneratorException( $"Function '{calleeName}' not found" );
        }

        private IExpression CreateBinaryOperatorNode( IExpression lhs, BinaryopContext op, IExpression rhs )
        {
            switch( op.OpToken.Type )
            {
            case LEFTANGLE:
                return new BinaryOperatorExpression( op.GetSourceSpan( ), lhs, BuiltInOperatorKind.Less, rhs );

            case CARET:
                return new BinaryOperatorExpression( op.GetSourceSpan( ), lhs, BuiltInOperatorKind.Pow, rhs );

            case PLUS:
                return new BinaryOperatorExpression( op.GetSourceSpan( ), lhs, BuiltInOperatorKind.Add, rhs );

            case MINUS:
                return new BinaryOperatorExpression( op.GetSourceSpan( ), lhs, BuiltInOperatorKind.Subtract, rhs );

            case ASTERISK:
                return new BinaryOperatorExpression( op.GetSourceSpan( ), lhs, BuiltInOperatorKind.Multiply, rhs );

            case SLASH:
                return new BinaryOperatorExpression( op.GetSourceSpan( ), lhs, BuiltInOperatorKind.Divide, rhs );

            case ASSIGN:
                return new BinaryOperatorExpression( op.GetSourceSpan( ), lhs, BuiltInOperatorKind.Assign, rhs );

            #region UserBinaryOpExpression
            default:
                {
                    // User defined op?
                    var opKind = RuntimeState.GetBinOperatorInfo( op.OpToken.Type ).Kind;
                    if( opKind != OperatorKind.InfixLeftAssociative && opKind != OperatorKind.InfixRightAssociative )
                    {
                        throw new CodeGeneratorException( $"Invalid binary operator '{op.OpToken.Text}'" );
                    }

                    string calleeName = CreateBinaryFunctionName( op.OpToken );
                    Prototype callTarget = FindCallTarget( calleeName );
                    return new FunctionCallExpression( op.GetSourceSpan( ), callTarget, lhs, rhs );
                }
                #endregion
            }
        }

        private static string CreateUnaryFunctionName( IToken opToken )
        {
            return $"$unary${opToken.Text}";
        }

        private static string CreateBinaryFunctionName( IToken opToken )
        {
            return $"$binary${opToken.Text}";
        }

        private IAstNode BuildPrototype( PrototypeContext context, string name )
        {
            var parameters = from param in context.Parameters
                             select new ParameterDeclaration( param.Span, param.Name, param.Index );

            var retVal = new Prototype( context.GetSourceSpan( ), name, parameters );

            // block second incompatible declaration to prevent issues with in any definitions that may be using it
            if( RuntimeState.FunctionDeclarations.TryGetValue( name, out Prototype existingPrototype ) )
            {
                if( existingPrototype.Parameters.Count != retVal.Parameters.Count )
                {
                    throw new CodeGeneratorException( "Declaration incompatible with previous declaration" );
                }
            }

            RuntimeState.FunctionDeclarations.AddOrReplaceItem( retVal );
            return retVal;
        }

        private void BeginFunctionDefinition( )
        {
            LocalVariables.Clear( );
            LocalVarIndex = 0;
        }

        private void Push( LocalVariableDeclaration variable )
        {
            NamedValues[ variable.Name ] = variable;
            LocalVariables.Add( variable );
        }

        private void Push( ParameterDeclaration param )
        {
            NamedValues[ param.Name ] = param;
        }

        private int LocalVarIndex;
        private readonly List<LocalVariableDeclaration> LocalVariables = new List<LocalVariableDeclaration>();
        private readonly ScopeStack<IVariableDeclaration> NamedValues = new ScopeStack<IVariableDeclaration>( );
        private readonly DynamicRuntimeState RuntimeState;
    }
}
