﻿// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Instructions;
using Llvm.NET.JIT;
using Llvm.NET.Transforms;
using Llvm.NET.Values;

using static Kaleidoscope.Grammar.KaleidoscopeParser;

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

namespace Kaleidoscope
{
    /// <summary>Static extension methods to perform LLVM IR Code generation from the Kaleidoscope AST</summary>
    internal sealed class CodeGenerator
        : KaleidoscopeBaseVisitor<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<Value>
    {
        // <Initialization>
        public CodeGenerator( DynamicRuntimeState globalState )
        {
            RuntimeState = globalState;
            Context = new Context( );
            JIT = new KaleidoscopeJIT( );
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
            FunctionPrototypes = new PrototypeCollection( );
            FunctionModuleMap = new Dictionary<string, IJitModuleHandle>( );
            NamedValues = new Dictionary<string, Value>( );
        }
        // </Initialization>

        public bool DisableOptimizations { get; set; }

        public void Dispose( )
        {
            JIT.Dispose( );
            Context.Dispose( );
        }

        // <Generate>
        public Value Generate( Parser parser, IParseTree tree, DiagnosticRepresentations additionalDiagnostics )
        {
            if( parser.NumberOfSyntaxErrors > 0 )
            {
                return null;
            }

            return Visit( tree );
        }
        // </Generate>

        public override Value VisitParenExpression( [NotNull] ParenExpressionContext context )
        {
            return context.Expression.Accept( this );
        }

        // <VisitConstExpression>
        public override Value VisitConstExpression( [NotNull] ConstExpressionContext context )
        {
            return Context.CreateConstant( context.Value );
        }
        // </VisitConstExpression>

        // <VisitVariableExpression>
        public override Value VisitVariableExpression( [NotNull] VariableExpressionContext context )
        {
            string varName = context.Name;
            if( !NamedValues.TryGetValue( varName, out Value value ) )
            {
                throw new CodeGeneratorException( $"Unknown variable name: {context}" );
            }

           return value;
        }
        // </VisitVariableExpression>

        public override Value VisitFunctionCallExpression( [NotNull] FunctionCallExpressionContext context )
        {
            var function = FindCallTarget( context.CaleeName );
            if( function == null )
            {
                throw new CodeGeneratorException( $"function '{context.CaleeName}' is unknown" );
            }

            var args = context.Args.Select( ctx => ctx.Accept( this ) ).ToArray( );
            return InstructionBuilder.Call( function, args ).RegisterName( "calltmp" );
        }

        // <FunctionDeclarations>
        public override Value VisitExternalDeclaration( [NotNull] ExternalDeclarationContext context )
        {
            return context.Signature.Accept( this );
        }

        public override Value VisitFunctionPrototype( [NotNull] FunctionPrototypeContext context )
        {
            return GetOrDeclareFunction( new Prototype( context ) );
        }
        // </FunctionDeclarations>

        // <VisitFunctionDefinition>
        public override Value VisitFunctionDefinition( [NotNull] FunctionDefinitionContext context )
        {
            return DefineFunction( ( Function )context.Signature.Accept( this )
                                 , context.BodyExpression
                                 ).Function;
        }
        // </VisitFunctionDefinition>

        // <VisitTopLevelExpression>
        public override Value VisitTopLevelExpression( [NotNull] TopLevelExpressionContext context )
        {
            var proto = new Prototype( $"anon_expr_{AnonNameIndex++}" );
            var function = GetOrDeclareFunction( proto, isAnonymous: true );

            var (_, jitHandle) = DefineFunction( function, context.expression( ) );

            var nativeFunc = JIT.GetDelegateForFunction<AnonExpressionFunc>( proto.Identifier.Name );
            var retVal = Context.CreateConstant( nativeFunc( ) );
            FunctionModuleMap.Remove( function.Name );
            JIT.RemoveModule( jitHandle );
            return retVal;
        }
        // </VisitTopLevelExpression>

        // <VisitExpression>
        public override Value VisitExpression( [NotNull] ExpressionContext context )
        {
            // Expression: PrimaryExpression (op expression)*
            // the sub-expressions are in evaluation order
            var lhs = context.Atom.Accept( this );
            foreach( var (op, rhs) in context.OperatorExpressions )
            {
                lhs = EmitBinaryOperator( lhs, op, rhs );
            }

            return lhs;
        }
        // </VisitExpression>

        protected override Value DefaultResult => null;

        // <EmitBinaryOperator>
        private Value EmitBinaryOperator( Value lhs, BinaryopContext op, IParseTree rightTree )
        {
            var rhs = rightTree.Accept( this );
            if( lhs == null || rhs == null )
            {
                return null;
            }

            switch( op.Token.Type )
            {
            case LEFTANGLE:
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan, lhs, rhs )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case CARET:
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow, lhs, rhs )
                                             .RegisterName( "powtmp" );
                }

            case PLUS:
                return InstructionBuilder.FAdd( lhs, rhs ).RegisterName( "addtmp" );

            case MINUS:
                return InstructionBuilder.FSub( lhs, rhs ).RegisterName( "subtmp" );

            case ASTERISK:
                return InstructionBuilder.FMul( lhs, rhs ).RegisterName( "multmp" );

            case SLASH:
                return InstructionBuilder.FDiv( lhs, rhs ).RegisterName( "divtmp" );

            default:
                throw new CodeGeneratorException( $"Invalid binary operator {op.Token.Text}" );
            }
        }
        // </EmitBinaryOperator>

        // <InitializeModuleAndPassManager>
        private void InitializeModuleAndPassManager( )
        {
            Module = Context.CreateBitcodeModule( );
            Module.Layout = JIT.TargetMachine.TargetData;
            FunctionPassManager = new FunctionPassManager( Module );
            FunctionPassManager.AddInstructionCombiningPass( )
                               .AddReassociatePass( )
                               .AddGVNPass( )
                               .AddCFGSimplificationPass( )
                               .Initialize( );
        }
        // </InitializeModuleAndPassManager>

        // <FindCallTarget>
        private Function FindCallTarget( string name )
        {
            // lookup the prototype for the function to get the signature
            // and create a declaration in this module
            if( FunctionPrototypes.TryGetValue( name, out var signature ) )
            {
                return GetOrDeclareFunction( signature );
            }

            Function retVal = Module.GetFunction( name );
            if( retVal != null )
            {
                return retVal;
            }

            return null;
        }
        // </FindCallTarget>

        // <GetOrDeclareFunction>
        private Function GetOrDeclareFunction( Prototype prototype, bool isAnonymous = false )
        {
            var function = Module.GetFunction( prototype.Identifier.Name );
            if( function != null )
            {
                return function;
            }

            var llvmSignature = Context.GetFunctionType( Context.DoubleType, prototype.Parameters.Select( _ => Context.DoubleType ) );

            var retVal = Module.AddFunction( prototype.Identifier.Name, llvmSignature );
            retVal.Linkage( Linkage.External );

            int index = 0;
            foreach( var argId in prototype.Parameters )
            {
                retVal.Parameters[ index ].Name = argId.Name;
                ++index;
            }

            if( !isAnonymous )
            {
                FunctionPrototypes.AddOrReplaceItem( prototype );
            }

            return retVal;
        }
        // </GetOrDeclareFunction>

        // <DefineFunction>
        private (Function Function, IJitModuleHandle JitHandle) DefineFunction( Function function, ExpressionContext body )
        {
            if( !function.IsDeclaration )
            {
                throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
            }

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

            var funcReturn = body.Accept( this );
            if( funcReturn == null )
            {
                function.EraseFromParent( );
                return (null, default);
            }

            InstructionBuilder.Return( funcReturn );
            function.Verify( );

            if( !DisableOptimizations )
            {
                FunctionPassManager.Run( function );
            }

            var jitHandle = JIT.AddModule( Module );
            FunctionModuleMap.Add( function.Name, jitHandle );
            InitializeModuleAndPassManager( );
            return (function, jitHandle);
        }
        // </DefineFunction>

        // <PrivateMembers>
        private readonly DynamicRuntimeState RuntimeState;
        private static int AnonNameIndex;
        private readonly Context Context;
        private BitcodeModule Module;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly IDictionary<string, Value> NamedValues;
        private readonly KaleidoscopeJIT JIT;
        private readonly Dictionary<string, IJitModuleHandle> FunctionModuleMap;
        private FunctionPassManager FunctionPassManager;
        private readonly PrototypeCollection FunctionPrototypes;

        /// <summary>Delegate type to allow execution of a JIT'd TopLevelExpression</summary>
        /// <returns>Result of evaluating the expression</returns>
        [UnmanagedFunctionPointer( System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private delegate double AnonExpressionFunc( );
        // </PrivateMembers>
    }
}
