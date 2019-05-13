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
using Llvm.NET;
using Llvm.NET.Instructions;
using Llvm.NET.JIT;
using Llvm.NET.Transforms;
using Llvm.NET.Values;

using ConstantExpression = Kaleidoscope.Grammar.AST.ConstantExpression;

namespace Kaleidoscope.Chapter4
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : AstVisitorBase<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<Value>
    {
        #region Initialization
        public CodeGenerator( DynamicRuntimeState globalState, bool disableOptimization = false )
            : base( null )
        {
            if( globalState.LanguageLevel > LanguageLevel.SimpleExpressions )
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof( globalState ) );
            }

            RuntimeState = globalState;
            Context = new Context( );
            DisableOptimizations = disableOptimization;
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
        }
        #endregion

        public void Dispose( )
        {
            JIT.Dispose( );
            Module.Dispose( );
            Context.Dispose( );
        }

        #region Generate
        public Value Generate( IAstNode ast, Action<CodeGeneratorException> codeGenerationErroHandler )
        {
            try
            {
                // Prototypes, including extern are ignored as AST generation
                // adds them to the RuntimeState so that already has the declarations
                if( !( ast is FunctionDefinition definition ) )
                {
                    return null;
                }

                InitializeModuleAndPassManager( );

                // Destroy any previously generated module for this function.
                // This allows re-definition as the new module will provide the
                // implementation. This is needed, otherwise both the MCJIT
                // and OrcJit engines will resolve to the original module, despite
                // claims to the contrary in the official tutorial text. (Though,
                // to be fair it may have been true in the original JIT and might
                // still be true for the interpreter)
                if( FunctionModuleMap.Remove( definition.Name, out IJitModuleHandle handle ) )
                {
                    JIT.RemoveModule( handle );
                }

                var function = ( IrFunction )definition.Accept( this );
                var jitHandle = JIT.AddModule( function.ParentModule );
                if( definition.IsAnonymous )
                {
                    var nativeFunc = JIT.GetFunctionDelegate<KaleidoscopeJIT.CallbackHandler0>( function.Name );
                    var retVal = Context.CreateConstant( nativeFunc( ) );
                    JIT.RemoveModule( jitHandle );
                    return retVal;
                }

                FunctionModuleMap.Add( function.Name, jitHandle );
                return function;
            }
            catch( CodeGeneratorException ex ) when( codeGenerationErroHandler != null )
            {
                codeGenerationErroHandler( ex );
                return null;
            }
        }
        #endregion

        #region ConstantExpression
        public override Value Visit( ConstantExpression constant )
        {
            return Context.CreateConstant( constant.Value );
        }
        #endregion

        #region BinaryOperatorExpression
        public override Value Visit( BinaryOperatorExpression binaryOperator )
        {
            switch( binaryOperator.Op )
            {
            case BuiltInOperatorKind.Less:
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan, binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case BuiltInOperatorKind.Pow:
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow, binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) )
                                             .RegisterName( "powtmp" );
                }

            case BuiltInOperatorKind.Add:
                return InstructionBuilder.FAdd( binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) ).RegisterName( "addtmp" );

            case BuiltInOperatorKind.Subtract:
                return InstructionBuilder.FSub( binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) ).RegisterName( "subtmp" );

            case BuiltInOperatorKind.Multiply:
                return InstructionBuilder.FMul( binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) ).RegisterName( "multmp" );

            case BuiltInOperatorKind.Divide:
                return InstructionBuilder.FDiv( binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) ).RegisterName( "divtmp" );

            default:
                throw new CodeGeneratorException( $"ICE: Invalid binary operator {binaryOperator.Op}" );
            }
        }
        #endregion

        #region FunctionCallExpression
        public override Value Visit( FunctionCallExpression functionCall )
        {
            string targetName = functionCall.FunctionPrototype.Name;
            IrFunction function;

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
        #endregion

        #region FunctionDefinition
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
                foreach( var param in definition.Signature.Parameters )
                {
                    NamedValues[ param.Name ] = function.Parameters[ param.Index ];
                }

                var funcReturn = definition.Body.Accept( this );
                InstructionBuilder.Return( funcReturn );
                function.Verify( );

                FunctionPassManager.Run( function );
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
        #endregion

        #region InitializeModuleAndPassManager
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
        #endregion

        #region GetOrDeclareFunction

        // Retrieves a Function" for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private IrFunction GetOrDeclareFunction( Prototype prototype )
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
        #endregion

        #region PrivateMembers
        private readonly DynamicRuntimeState RuntimeState;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly IDictionary<string, Value> NamedValues = new Dictionary<string, Value>( );
        private FunctionPassManager FunctionPassManager;
        private readonly bool DisableOptimizations;
        private BitcodeModule Module;
        private readonly KaleidoscopeJIT JIT = new KaleidoscopeJIT( );
        private readonly Dictionary<string, IJitModuleHandle> FunctionModuleMap = new Dictionary<string, IJitModuleHandle>( );
        #endregion
    }
}
