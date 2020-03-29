// -----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Values;

using ConstantExpression = Kaleidoscope.Grammar.AST.ConstantExpression;

namespace Kaleidoscope.Chapter3
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : AstVisitorBase<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<Value>
    {
        #region Initialization
        public CodeGenerator( DynamicRuntimeState globalState )
            : base( null )
        {
            globalState.ValidateNotNull( nameof( globalState ) );
            if( globalState.LanguageLevel > LanguageLevel.SimpleExpressions )
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof( globalState ) );
            }

            RuntimeState = globalState;
            Context = new Context( );
            Module = Context.CreateBitcodeModule( "Kaleidoscope" );
            InstructionBuilder = new InstructionBuilder( Context );
        }
        #endregion

        public BitcodeModule Module { get; }

        public void Dispose( )
        {
            Module.Dispose( );
            Context.Dispose( );
        }

        #region Generate
        public Value? Generate( IAstNode ast, Action<CodeGeneratorException> codeGenerationErroHandler )
        {
            ast.ValidateNotNull( nameof( ast ) );
            codeGenerationErroHandler.ValidateNotNull( nameof( codeGenerationErroHandler ) );
            try
            {
                // Prototypes, including extern are ignored as AST generation
                // adds them to the RuntimeState so that already has the declarations
                return ( ast is FunctionDefinition ) ? ast.Accept( this ) : null;
            }
            catch( CodeGeneratorException ex )
            {
                codeGenerationErroHandler( ex );
                return null;
            }
        }
        #endregion

        #region ConstantExpression
        public override Value? Visit( ConstantExpression constant )
        {
            constant.ValidateNotNull( nameof( constant ) );
            return Context.CreateConstant( constant.Value );
        }
        #endregion

        #region BinaryOperatorExpression
        public override Value? Visit( BinaryOperatorExpression binaryOperator )
        {
            binaryOperator.ValidateNotNull( nameof( binaryOperator ) );
            switch( binaryOperator.Op )
            {
            case BuiltInOperatorKind.Less:
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan
                                                        , binaryOperator.Left.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                                        , binaryOperator.Right.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                                        ).RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case BuiltInOperatorKind.Pow:
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow
                                                  , binaryOperator.Left.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                                  , binaryOperator.Right.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                                  ).RegisterName( "powtmp" );
                }

            case BuiltInOperatorKind.Add:
                return InstructionBuilder.FAdd( binaryOperator.Left.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "addtmp" );

            case BuiltInOperatorKind.Subtract:
                return InstructionBuilder.FSub( binaryOperator.Left.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "subtmp" );

            case BuiltInOperatorKind.Multiply:
                return InstructionBuilder.FMul( binaryOperator.Left.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "multmp" );

            case BuiltInOperatorKind.Divide:
                return InstructionBuilder.FDiv( binaryOperator.Left.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "divtmp" );

            default:
                throw new CodeGeneratorException( $"ICE: Invalid binary operator {binaryOperator.Op}" );
            }
        }
        #endregion

        #region FunctionCallExpression
        public override Value? Visit( FunctionCallExpression functionCall )
        {
            functionCall.ValidateNotNull( nameof( functionCall ) );
            string targetName = functionCall.FunctionPrototype.Name;

            IrFunction? function;
            if( RuntimeState.FunctionDeclarations.TryGetValue( targetName, out Prototype target ) )
            {
                function = GetOrDeclareFunction( target );
            }
            else if( !Module.TryGetFunction( targetName, out function ) )
            {
                throw new CodeGeneratorException( $"Definition for function {targetName} not found" );
            }

            var args = ( from expr in functionCall.Arguments
                         select expr.Accept( this ) ?? throw new CodeGeneratorException(ExpectValidExpr)
                       ).ToArray();

            return InstructionBuilder.Call( function, args ).RegisterName( "calltmp" );
        }
        #endregion

        #region FunctionDefinition
        public override Value? Visit( FunctionDefinition definition )
        {
            definition.ValidateNotNull( nameof( definition ) );
            var function = GetOrDeclareFunction( definition.Signature );
            if( !function.IsDeclaration )
            {
                throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
            }

            try
            {
                var entryBlock = function.AppendBasicBlock( "entry" );
                InstructionBuilder.PositionAtEnd( entryBlock );
                NamedValues.Clear( );
                foreach( var param in definition.Signature.Parameters )
                {
                    NamedValues[ param.Name ] = function.Parameters[ param.Index ];
                }

                var funcReturn = definition.Body.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidFunc );
                InstructionBuilder.Return( funcReturn );
                function.Verify( );
                return function;
            }
            catch( CodeGeneratorException )
            {
                function.EraseFromParent( );
                throw;
            }
        }
        #endregion

        #region VariableReferenceExpression
        public override Value? Visit( VariableReferenceExpression reference )
        {
            reference.ValidateNotNull( nameof( reference ) );
            if( !NamedValues.TryGetValue( reference.Name, out Value? value ) )
            {
                // Source input is validated by the parser and AstBuilder, therefore
                // this is the result of an internal error in the generator rather
                // then some sort of user error.
                throw new CodeGeneratorException( $"ICE: Unknown variable name: {reference.Name}" );
            }

            return value;
        }
        #endregion

        #region GetOrDeclareFunction

        // Retrieves a Function for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private IrFunction GetOrDeclareFunction( Prototype prototype )
        {
            if( Module.TryGetFunction( prototype.Name, out IrFunction? function ) )
            {
                return function;
            }

            var llvmSignature = Context.GetFunctionType( Context.DoubleType, prototype.Parameters.Select( _ => Context.DoubleType ) );
            var retVal = Module.AddFunction( prototype.Name, llvmSignature );

            int index = 0;
            foreach( var argId in prototype.Parameters )
            {
                retVal.Parameters[ index ].Name = argId.Name;
                ++index;
            }

            return retVal;
        }
        #endregion

        private const string ExpectValidExpr = "Expected a valid expression";
        private const string ExpectValidFunc = "Expected a valid function";

        #region PrivateMembers
        private readonly DynamicRuntimeState RuntimeState;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly IDictionary<string, Value> NamedValues = new Dictionary<string, Value>( );
        #endregion
    }
}
