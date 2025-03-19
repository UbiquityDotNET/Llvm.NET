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

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Values;

using ConstantExpression = Kaleidoscope.Grammar.AST.ConstantExpression;

namespace Kaleidoscope.Chapter3_5
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : AstVisitorBase<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<Value>
    {
        public CodeGenerator(DynamicRuntimeState globalState)
            : base( null )
        {
            ArgumentNullException.ThrowIfNull( globalState );

            if(globalState.LanguageLevel > LanguageLevel.SimpleExpressions)
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof( globalState ) );
            }

            RuntimeState = globalState;
            Context = new Context();
            Module = Context.CreateBitcodeModule( "Kaleidoscope" );
            InstructionBuilder = new InstructionBuilder( Context );
        }

        public void Dispose()
        {
            Module.Dispose();
            InstructionBuilder.Dispose();
            Context.Dispose();
        }

        public OptionalValue<Value> Generate(IAstNode ast)
        {
            ArgumentNullException.ThrowIfNull( ast );

            // Prototypes, including extern are ignored as AST generation
            // adds them to the RuntimeState so that already has the declarations
            // They are looked up and added to the module as extern if not already
            // present if they are called.
            if(ast is not FunctionDefinition definition)
            {
                return default;
            }

            var function = definition.Accept( this ) as Function ?? throw new CodeGeneratorException(ExpectValidFunc);
            if(!function.ParentModule.Verify(out string msg))
            {
                throw new CodeGeneratorException(msg);
            }

            return OptionalValue.Create<Value>( function );
        }

        public override Value? Visit(ConstantExpression constant)
        {
            ArgumentNullException.ThrowIfNull( constant );

            return Context.CreateConstant( constant.Value );
        }

        public override Value? Visit(BinaryOperatorExpression binaryOperator)
        {
            ArgumentNullException.ThrowIfNull( binaryOperator );

            switch(binaryOperator.Op)
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

        public override Value? Visit(FunctionCallExpression functionCall)
        {
            ArgumentNullException.ThrowIfNull( functionCall );

            string targetName = functionCall.FunctionPrototype.Name;

            Function? function;
            if(RuntimeState.FunctionDeclarations.TryGetValue( targetName, out Prototype? target ))
            {
                function = GetOrDeclareFunction( target );
            }
            else if(!Module.TryGetFunction( targetName, out function ))
            {
                throw new CodeGeneratorException( $"Definition for function {targetName} not found" );
            }

            var args = ( from expr in functionCall.Arguments
                         select expr.Accept( this ) ?? throw new CodeGeneratorException(ExpectValidExpr)
                       ).ToArray();

            return InstructionBuilder.Call( function, args ).RegisterName( "calltmp" );
        }

        #region FunctionDefinition
        public override Value? Visit(FunctionDefinition definition)
        {
            ArgumentNullException.ThrowIfNull( definition );

            var function = GetOrDeclareFunction( definition.Signature );
            if(!function.IsDeclaration)
            {
                throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
            }

            try
            {
                var entryBlock = function.AppendBasicBlock( "entry" );
                InstructionBuilder.PositionAtEnd( entryBlock );
                NamedValues.Clear();
                foreach(var param in definition.Signature.Parameters)
                {
                    NamedValues[ param.Name ] = function.Parameters[ param.Index ];
                }

                var funcReturn = definition.Body.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidFunc );
                InstructionBuilder.Return( funcReturn );
                function.Verify();

                // pass pipeline is run against the module
                using var errInfo = function.ParentModule.TryRunPasses( PassNames );
                return errInfo.Success ? (Value)function : throw new CodeGeneratorException( errInfo.ToString() );
            }
            catch(CodeGeneratorException)
            {
                function.EraseFromParent();
                throw;
            }
        }
        #endregion

        public override Value? Visit(VariableReferenceExpression reference)
        {
            ArgumentNullException.ThrowIfNull( reference );

            if(!NamedValues.TryGetValue( reference.Name, out Value? value ))
            {
                // Source input is validated by the parser and AstBuilder, therefore
                // this is the result of an internal error in the generator rather
                // then some sort of user error.
                throw new CodeGeneratorException( $"ICE: Unknown variable name: {reference.Name}" );
            }

            return value;
        }

        #region GetOrDeclareFunction

        // Retrieves a Function for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private Function GetOrDeclareFunction(Prototype prototype)
        {
            if(Module.TryGetFunction( prototype.Name, out Function? function ))
            {
                return function;
            }

            var llvmSignature = Context.GetFunctionType( returnType: Context.DoubleType, args: prototype.Parameters.Select( _ => Context.DoubleType ) );
            var retVal = Module.CreateFunction( prototype.Name, llvmSignature );
            retVal.AddAttribute( FunctionAttributeIndex.Function, prototype.IsExtern ? AttributeKind.BuiltIn : AttributeKind.NoBuiltIn );

            int index = 0;
            foreach(var argId in prototype.Parameters)
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
        private readonly Module Module;
        private readonly DynamicRuntimeState RuntimeState;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly Dictionary<string, Value> NamedValues = [];
        private static readonly string[] PassNames = [
            "default<O3>"
        ];
        #endregion
    }
}
