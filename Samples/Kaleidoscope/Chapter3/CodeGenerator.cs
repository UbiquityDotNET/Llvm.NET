// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Instructions;
using Llvm.NET.Values;

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

namespace Kaleidoscope
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    internal sealed class CodeGenerator
        : AstVisitorBase<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<Value>
    {
        // <Initialization>
        public CodeGenerator( DynamicRuntimeState globalState )
            : base(null)
        {
            if( globalState.LanguageLevel > LanguageLevel.SimpleExpressions )
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof(globalState) );
            }

            RuntimeState = globalState;
            Context = new Context( );
            Module = Context.CreateBitcodeModule( "Kaleidoscope" );
            InstructionBuilder = new InstructionBuilder( Context );
        }
        // </Initialization>

        public BitcodeModule Module { get; private set; }

        public void Dispose( )
        {
            Context.Dispose( );
        }

        // <Generate>
        public Value Generate( IAstNode ast )
        {
            // Prototypes, including extern are ignored as AST generation
            // adds them to the RuntimeState so that already has the declarations
            if( !( ast is FunctionDefinition definition ) )
            {
                return null;
            }

            return ast.Accept( this );
        }
        // </Generate>

        // <ConstantExpression>
        public override Value Visit( Kaleidoscope.Grammar.AST.ConstantExpression constant )
        {
            return Context.CreateConstant( constant.Value );
        }
        // </ConstantExpression>

        // <BinaryOperatorExpression>
        public override Value Visit( BinaryOperatorExpression binaryOperator )
        {
            var lhs = binaryOperator.Left.Accept( this );
            var rhs = binaryOperator.Right.Accept( this );

            switch( binaryOperator.Op )
            {
            case BuiltInOperatorKind.Less:
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan, lhs, rhs )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case BuiltInOperatorKind.Pow:
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow, lhs, rhs )
                                             .RegisterName( "powtmp" );
                }

            case BuiltInOperatorKind.Add:
                return InstructionBuilder.FAdd( lhs, rhs ).RegisterName( "addtmp" );

            case BuiltInOperatorKind.Subtract:
                return InstructionBuilder.FSub( lhs, rhs ).RegisterName( "subtmp" );

            case BuiltInOperatorKind.Multiply:
                return InstructionBuilder.FMul( lhs, rhs ).RegisterName( "multmp" );

            case BuiltInOperatorKind.Divide:
                return InstructionBuilder.FDiv( lhs, rhs ).RegisterName( "divtmp" );

            default:
                throw new CodeGeneratorException( $"ICE: Invalid binary operator {binaryOperator.Op}" );
            }
        }
        // </BinaryOperatorExpression>

        // <FunctionCallExpression>
        public override Value Visit( FunctionCallExpression functionCall )
        {
            string targetName = functionCall.FunctionPrototype.Name;
            Function function;
            // try for an extern function declaration
            if( RuntimeState.FunctionDeclarations.TryGetValue( targetName, out Prototype target ) )
            {
                function = GetOrDeclareFunction( target );
            }
            else
            {
                function = Module.GetFunction( targetName ) ?? throw new CodeGeneratorException( $"Definition for function {targetName} not found" );
            }

            var args = functionCall.Arguments.Select( ctx => ctx.Accept( this ) ).ToArray( );
            return InstructionBuilder.Call( function, args ).RegisterName( "calltmp" );
        }
        // </FunctionCallExpression>

        // <FunctionDefinition>
        public override Value Visit( FunctionDefinition definition )
        {
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
                foreach( var arg in function.Parameters )
                {
                    NamedValues[ arg.Name ] = arg;
                }

                var funcReturn = definition.Body.Accept( this );
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
        // </FunctionDefinition>

        // <VariableReferenceExpression>
        public override Value Visit( VariableReferenceExpression reference )
        {
            if( !NamedValues.TryGetValue( reference.Name, out Value value ) )
            {
                // Source input is validated by the parser and AstBuilder, therefore
                // this is the result of an internal error in the generator rather
                // then some sort of user error.
                throw new CodeGeneratorException( $"ICE: Unknown variable name: {reference.Name}" );
            }

            return value;
        }
        // </VariableReferenceExpression>

        // <GetOrDeclareFunction>
        // Retrieves a Function" for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private Function GetOrDeclareFunction( Prototype prototype )
        {
            var function = Module.GetFunction( prototype.Name );
            if( function != null )
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
        // </GetOrDeclareFunction>

        // <PrivateMembers>
        private readonly DynamicRuntimeState RuntimeState;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly IDictionary<string, Value> NamedValues = new Dictionary<string, Value>( );
        // </PrivateMembers>
    }
}
