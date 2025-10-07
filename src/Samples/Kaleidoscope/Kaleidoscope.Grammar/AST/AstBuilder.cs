// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using Ubiquity.NET.ANTLR.Utils;
using Ubiquity.NET.Runtime.Utils;

using static Kaleidoscope.Grammar.ANTLR.KaleidoscopeParser;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Parse tree Visitor to construct the AST from an ANTLR generated parse tree for the Kaleidoscope language</summary>
    internal sealed class AstBuilder
        : Kaleidoscope.Grammar.ANTLR.KaleidoscopeBaseVisitor<IAstNode>
    {
        public AstBuilder( DynamicRuntimeState globalState )
        {
            RuntimeState = globalState;
        }

        public override IAstNode VisitErrorNode( IErrorNode node )
        {
            return new ErrorNode( node.GetSourceRange(), (int)DiagnosticCode.SyntaxError, $"Syntax Error: {node}" );
        }

        public override IAstNode VisitParenExpression( ParenExpressionContext context )
        {
            return context.Expression.Accept( this );
        }

        public override IAstNode VisitConstExpression( ConstExpressionContext context )
        {
            return new ConstantExpression( context.GetSourceRange(), context.Value );
        }

        public override IAstNode VisitVariableExpression( VariableExpressionContext context )
        {
            string varName = context.Name;
            return NamedValues.TryGetValue( varName, out IVariableDeclaration? declaration )
                ? new VariableReferenceExpression( context.GetSourceRange(), declaration )
                : new ErrorNode( context.GetSourceRange(), (int)DiagnosticCode.UnknownVariable, $"Unknown variable name: {varName}" );
        }

        public override IAstNode VisitFunctionCallExpression( FunctionCallExpressionContext context )
        {
            Prototype? function = FindCallTarget( context.CaleeName );
            if(function is null)
            {
                return new ErrorNode( context.GetSourceRange(), (int)DiagnosticCode.InvokeUnknownFunction, $"Call to unknown function '{context.CaleeName}'" );
            }

            var argNodes = ( from expCtx in context.expression( )
                             select expCtx.Accept( this )
                           ).ToList();

            foreach(var arg in argNodes)
            {
                if(arg is not IExpression)
                {
                    return arg;
                }
            }

            return new FunctionCallExpression( context.GetSourceRange(), function, argNodes.Cast<IExpression>() );
        }

        public override IAstNode VisitExpression( ExpressionContext context )
        {
            // Expression: PrimaryExpression (op expression)*
            // where the sub-expressions are in evaluation order
            IAstNode lhsNode = context.Atom.Accept( this );

            foreach(var (op, rhs) in context.OperatorExpressions)
            {
                if(lhsNode is not IExpression lhsExp)
                {
                    return lhsNode;
                }

                var rhsNode = rhs.Accept( this );
                if(rhsNode is not IExpression rhsExp)
                {
                    return rhsNode;
                }

                lhsNode = CreateBinaryOperatorNode( lhsExp, op, rhsExp );
            }

            return lhsNode;
        }

        public override IAstNode VisitExternalDeclaration( ExternalDeclarationContext context )
        {
            var retVal = ( Prototype )context.Signature.Accept( this );
            var errors = retVal.CollectErrors( );
            if(errors.Length == 0)
            {
                RuntimeState.FunctionDeclarations.AddOrReplaceItem( retVal );
            }

            return retVal;
        }

        public override IAstNode VisitFunctionDefinition( FunctionDefinitionContext context )
        {
            BeginFunctionDefinition();

            var sig = ( Prototype )context.Signature.Accept( this );
            foreach(var param in sig.Parameters)
            {
                Push( param );
            }

            var body = context.BodyExpression.Accept( this );
            var errors = body.CollectErrors( );

            // test for a non-expression (ErrorNode)
            if(body is not IExpression exp)
            {
                return body;
            }

            var retVal = new FunctionDefinition( context.GetSourceRange( )
                                               , sig
                                               , exp
                                               , LocalVariables.ToImmutableArray( )
                                               );

            // only add valid definitions to the runtime state.
            if(errors.Length == 0)
            {
                RuntimeState.FunctionDefinitions.AddOrReplaceItem( retVal );
            }
            else
            {
                // remove the prototype implicitly added for this definition
                // as the definition has errors
                RuntimeState.FunctionDeclarations.Remove( sig );
            }

            return retVal;
        }

        public override IAstNode VisitTopLevelExpression( TopLevelExpressionContext context )
        {
            BeginFunctionDefinition();
            var sig = new Prototype( context.GetSourceRange( ), RuntimeState.GenerateAnonymousName( ), true );
            var bodyNode = context.expression( ).Accept( this );
            var errors = bodyNode.CollectErrors();

            // only add valid definitions to the runtime state.
            if(errors.Length > 0 || bodyNode is not IExpression bodyExp)
            {
                return bodyNode;
            }

            var retVal = new FunctionDefinition( context.GetSourceRange( ), sig, bodyExp, true );
            RuntimeState.FunctionDefinitions.AddOrReplaceItem( retVal );
            return retVal;
        }

        #region UnaryOpExpression
        public override IAstNode VisitUnaryOpExpression( UnaryOpExpressionContext context )
        {
            // verify the operator was previously defined
            var opKind = RuntimeState.GetUnaryOperatorInfo( context.Op ).Kind;
            if(opKind == OperatorKind.None)
            {
                return new ErrorNode( context.GetSourceRange(), (int)DiagnosticCode.InvalidUnaryOp, $"invalid unary operator {context.Op}" );
            }

            string calleeName = CreateUnaryFunctionName( context.OpToken );
            var function = FindCallTarget( calleeName );
            if(function == null)
            {
                return new ErrorNode( context.GetSourceRange(), (int)DiagnosticCode.InvalidUnaryOpRef, $"reference to unknown unary operator function {calleeName}" );
            }

            var arg = context.Rhs.Accept( this );
            return arg is not IExpression exp ? arg : new FunctionCallExpression( context.GetSourceRange(), function, exp );
        }
        #endregion

        public override IAstNode VisitFullsrc( FullsrcContext context )
        {
            var children = from child in context.children
                           where child is not TopLevelSemicolonContext
                           select child.Accept( this );

            return new RootNode( context.GetSourceRange(), children );
        }

        public override IAstNode VisitConditionalExpression( ConditionalExpressionContext context )
        {
            var expressionSpan = context.GetSourceRange( );
            var condition = context.Condition.Accept( this );
            if(condition is not IExpression conditionExp)
            {
                return condition;
            }

            var thenClause = context.ThenExpression.Accept( this );
            if(thenClause is not IExpression thenExp)
            {
                return thenClause;
            }

            var elseClause = context.ElseExpression.Accept( this );
            if(elseClause is not IExpression elseExp)
            {
                return elseClause;
            }

            // compiler generated result variable supports building conditional
            // expressions without the need for SSA form using mutable variables
            // The result is assigned a value from both sides of the branch. In
            // pure SSA form this isn't needed as a PHI node would be used instead.
            var retVal = new ConditionalExpression( expressionSpan
                                                  , conditionExp
                                                  , thenExp
                                                  , elseExp
                                                  , new LocalVariableDeclaration( expressionSpan, $"$ifresult${LocalVarIndex++}", null, true)
                                                  );
            Push( retVal.ResultVariable );
            return retVal;
        }

        public override IAstNode VisitForExpression( ForExpressionContext context )
        {
            var initializer = ( LocalVariableDeclaration )context.Initializer.Accept( this );

            ForInExpression retVal;
            using(NamedValues.EnterScope())
            {
                Push( initializer );

                var conditionNode = context.EndExpression.Accept( this );
                if(conditionNode is not IExpression conditionExp)
                {
                    return conditionNode;
                }

                var stepNode = context.StepExpression?.Accept( this ) ?? new ConstantExpression( default, 1.0 );
                if(stepNode is not IExpression stepExp)
                {
                    return stepNode;
                }

                var bodyNode = context.BodyExpression.Accept( this );
                if(bodyNode is not IExpression bodyExp)
                {
                    return bodyNode;
                }

                retVal = new ForInExpression( context.GetSourceRange(), initializer, conditionExp, stepExp, bodyExp );
            }

            return retVal;
        }

        public override IAstNode VisitVarInExpression( VarInExpressionContext context )
        {
            using(NamedValues.EnterScope())
            {
                var localVariables = from initializer in context.Initiaizers
                                     select ( LocalVariableDeclaration )initializer.Accept( this );

                foreach(var local in localVariables)
                {
                    Push( local );
                }

                var bodyNode = context.Scope.Accept( this );
                return bodyNode is not IExpression bodyExp ? bodyNode : new VarInExpression( context.GetSourceRange(), localVariables, bodyExp );
            }
        }

        public override IAstNode VisitFunctionPrototype( FunctionPrototypeContext context )
        {
            return BuildPrototype( context, context.Name );
        }

        public override IAstNode VisitInitializer( InitializerContext context )
        {
            var value = ( IExpression )(context.Value?.Accept( this ) ?? new ConstantExpression( context.GetSourceRange( ), 0.0 ));
            return new LocalVariableDeclaration( context.GetSourceRange()
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

        protected override IAstNode DefaultResult => NullNode.Instance;

        private Prototype? FindCallTarget( string calleeName )
        {
            // search defined functions first as they override extern declarations
            if(RuntimeState.FunctionDefinitions.TryGetValue( calleeName, out FunctionDefinition? definition ))
            {
                return definition.Signature;
            }

            // search extern declarations
            return RuntimeState.FunctionDeclarations.TryGetValue( calleeName, out Prototype? declaration )
                 ? declaration
                 : null;
        }

        private IAstNode CreateBinaryOperatorNode( IExpression lhs, BinaryopContext op, IExpression rhs )
        {
            switch(op.OpToken.Type)
            {
            case LEFTANGLE:
                return new BinaryOperatorExpression( op.GetSourceRange(), lhs, BuiltInOperatorKind.Less, rhs );

            case CARET:
                return new BinaryOperatorExpression( op.GetSourceRange(), lhs, BuiltInOperatorKind.Pow, rhs );

            case PLUS:
                return new BinaryOperatorExpression( op.GetSourceRange(), lhs, BuiltInOperatorKind.Add, rhs );

            case MINUS:
                return new BinaryOperatorExpression( op.GetSourceRange(), lhs, BuiltInOperatorKind.Subtract, rhs );

            case ASTERISK:
                return new BinaryOperatorExpression( op.GetSourceRange(), lhs, BuiltInOperatorKind.Multiply, rhs );

            case SLASH:
                return new BinaryOperatorExpression( op.GetSourceRange(), lhs, BuiltInOperatorKind.Divide, rhs );

            case ASSIGN:
                return new BinaryOperatorExpression( op.GetSourceRange(), lhs, BuiltInOperatorKind.Assign, rhs );

            #region UserBinaryOpExpression
            default:
            {
                // User defined op?
                var opKind = RuntimeState.GetBinOperatorInfo( op.OpToken.Type ).Kind;
                if(opKind != OperatorKind.InfixLeftAssociative && opKind != OperatorKind.InfixRightAssociative)
                {
                    return new ErrorNode( op.GetSourceRange(), (int)DiagnosticCode.InvalidBinaryOp, $"Invalid binary operator '{op.OpToken.Text}'" );
                }

                string calleeName = CreateBinaryFunctionName( op.OpToken );
                Prototype? callTarget = FindCallTarget( calleeName );
                return callTarget is null
                    ? new ErrorNode( op.GetSourceRange(), (int)DiagnosticCode.UnaryOpNotFound, $"Unary operator function '{calleeName}' not found" )
                    : new FunctionCallExpression( op.GetSourceRange(), callTarget, lhs, rhs );
            }
            #endregion
            }
        }

        private static string CreateUnaryFunctionName( IToken opToken )
        {
            return $"unary-op${opToken.Text}";
        }

        private static string CreateBinaryFunctionName( IToken opToken )
        {
            return $"binary-op${opToken.Text}";
        }

        private IAstNode BuildPrototype( PrototypeContext context, string name )
        {
            if(string.IsNullOrWhiteSpace( name ))
            {
                name = context.Name;
            }

            var retVal = new Prototype( context.GetSourceRange()
                                      , name
                                      , false
                                      , context.Parent is ExternalDeclarationContext
                                      , context.Parameters.Select( p => new ParameterDeclaration( p.Span, p.Name, p.Index ))
                                      );

            // block second incompatible (name + arity ) declaration to prevent issues with in any definitions that may be using it
            if(RuntimeState.FunctionDeclarations.TryGetValue( name, out Prototype? existingPrototype ))
            {
                if(existingPrototype.Parameters.Count != retVal.Parameters.Count)
                {
                    return new ErrorNode( context.GetSourceRange(), (int)DiagnosticCode.IncompatibleRedclaration, "Declaration incompatible with previous declaration" );
                }
            }

            var errors = retVal.CollectErrors( );
            if(errors.Length == 0)
            {
                RuntimeState.FunctionDeclarations.AddOrReplaceItem( retVal );
            }

            return retVal;
        }

        private void BeginFunctionDefinition( )
        {
            LocalVariables.Clear();
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
        private readonly List<LocalVariableDeclaration> LocalVariables = [];
        private readonly ScopeStack<IVariableDeclaration> NamedValues = new( );
        private readonly DynamicRuntimeState RuntimeState;
    }
}
