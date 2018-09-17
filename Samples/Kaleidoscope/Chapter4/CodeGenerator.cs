// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Instructions;
using Llvm.NET.JIT;
using Llvm.NET.Transforms;
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
        public CodeGenerator( DynamicRuntimeState globalState, bool disableOptimization = false  )
            : base(null)
        {
            if( globalState.LanguageLevel > LanguageLevel.SimpleExpressions )
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof(globalState) );
            }

            RuntimeState = globalState;
            Context = new Context( );
            JIT = new KaleidoscopeJIT( );
            DisableOptimizations = disableOptimization;
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
        }
        // </Initialization>


        public void Dispose( )
        {
            JIT.Dispose( );
            Context.Dispose( );
        }

        // <Generate>
        public Value Generate( IAstNode ast )
        {
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
                // Destroy any previously generated module for this function.
                // This allows re-definition as the new module will provide the
                // implementation. This is needed, otherwise both the MCJIT
                // and OrcJit engines will resolve to the original module, despite
                // claims to the contrary in the official tutorial text. (Though,
                // to be fair it may have been true in the original JIT and might
                // still be true for the interpreter)
                if( FunctionModuleMap.Remove( function.Name, out IJitModuleHandle handle ) )
                {
                    JIT.RemoveModule( handle );
                }

                var basicBlock = function.AppendBasicBlock( "entry" );
                InstructionBuilder.PositionAtEnd( basicBlock );
                NamedValues.Clear( );
                foreach( var arg in function.Parameters )
                {
                    NamedValues[ arg.Name ] = arg;
                }

                var funcReturn = definition.Body.Accept( this );
                InstructionBuilder.Return( funcReturn );
                function.Verify( );

                FunctionPassManager.Run( function );

                var jitHandle = JIT.AddModule( Module );
                if( definition.IsAnonymous )
                {
                    var nativeFunc = JIT.GetFunctionDelegate<AnonExpressionFunc>( function.Name );
                    var retVal = Context.CreateConstant( nativeFunc( ) );
                    JIT.RemoveModule( jitHandle );
                    return retVal;
                }

                FunctionModuleMap.Add( function.Name, jitHandle );
                return function;
            }
            catch( CodeGeneratorException )
            {
                function.EraseFromParent( );
                throw;
            }
            finally
            {
                InitializeModuleAndPassManager( );
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

        // <InitializeModuleAndPassManager>
        private void InitializeModuleAndPassManager( )
        {
            Module = Context.CreateBitcodeModule( );
            Module.Layout = JIT.TargetMachine.TargetData;
            FunctionPassManager = new FunctionPassManager( Module );

            if( !DisableOptimizations )
            {
                FunctionPassManager.AddInstructionCombiningPass( )
                                   .AddReassociatePass( )
                                   .AddGVNPass( )
                                   .AddCFGSimplificationPass( );
            }

            FunctionPassManager.Initialize( );
        }
        // </InitializeModuleAndPassManager>

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
        private FunctionPassManager FunctionPassManager;
        private bool DisableOptimizations;
        private BitcodeModule Module;
        private readonly KaleidoscopeJIT JIT;
        private readonly Dictionary<string, IJitModuleHandle> FunctionModuleMap = new Dictionary<string, IJitModuleHandle>( );

        /// <summary>Delegate type to allow execution of a JIT'd TopLevelExpression</summary>
        /// <returns>Result of evaluating the expression</returns>
        [UnmanagedFunctionPointer( System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private delegate double AnonExpressionFunc( );
        // </PrivateMembers>
    }
}
