// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Instructions;
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
        public CodeGenerator( DynamicRuntimeState globalState, TargetMachine machine, bool disableOptimization = false )
        {
            RuntimeState = globalState;
            Context = new Context( );
            TargetMachine = machine;
            DisableOptimizations = disableOptimization;
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
            FunctionPrototypes = new PrototypeCollection( );
            NamedValues = new ScopeStack<Alloca>( );
            AnonymousFunctions = new List<Function>( );
        }
        // </Initialization>

        public BitcodeModule Module { get; private set; }

        public void Dispose( )
        {
            Context.Dispose( );
        }

        // <Generate>
        public Value Generate( Parser parser, IParseTree tree, DiagnosticRepresentations additionalDiagnostics )
        {
            if( parser.NumberOfSyntaxErrors > 0 )
            {
                return null;
            }

            Visit( tree );
            if( AnonymousFunctions.Count > 0 )
            {
                var mainFunction = Module.AddFunction( "main", Context.GetFunctionType( Context.VoidType ) );
                var block = mainFunction.AppendBasicBlock( "entry" );
                var irBuilder = new InstructionBuilder( block );
                var printdFunc = Module.AddFunction( "printd", Context.GetFunctionType( Context.DoubleType, Context.DoubleType ) );
                foreach( var anonFunc in AnonymousFunctions )
                {
                    var value = irBuilder.Call( anonFunc );
                    irBuilder.Call( printdFunc, value );
                }

                irBuilder.Return( );

                // Use always inline and Dead Code Elimination module passes to inline all of the
                // anonymous functions. This effectively strips all the calls just generated for main()
                // and inlines each of the anonymous functions directly into main, dropping the now
                // unused original anonymous functions all while retaining all of the original source
                // debug information locations.
                var mpm = new ModulePassManager( )
                          .AddAlwaysInlinerPass( )
                          .AddGlobalDCEPass( );
                mpm.Run( Module );
            }

            return null;
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
            if( !NamedValues.TryGetValue( varName, out Alloca value ) )
            {
                throw new CodeGeneratorException( $"Unknown variable name: {context}" );
            }

            return InstructionBuilder.Load( value )
                                     .RegisterName( varName );
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
                                 );
        }
        // </VisitFunctionDefinition>

        // <VisitTopLevelExpression>
        public override Value VisitTopLevelExpression( [NotNull] TopLevelExpressionContext context )
        {
            var function = GetOrDeclareFunction( new Prototype( $"anon_expr_{AnonNameIndex++}" )
                                               , isAnonymous: true
                                               );

            AnonymousFunctions.Add( function );
            return DefineFunction( function, context.expression( ) );
        }
        // </VisitTopLevelExpression>

        // <VisitExpression>
        public override Value VisitExpression( [NotNull] ExpressionContext context )
        {
            // Expression: PrimaryExpression (op expression)*
            // the sub-expressions are in evaluation order
            //
            // Special case the assignment operator as there isn't anything to emit
            // for the lhs expression. (If it was emitted it would be a load of the
            // value and assignment needs a place to store the value)
            Value lhs = null;
            var firstOp = context.GetChild<ITerminalNode>( 0 );
            if( context.IsAssignment )
            {
                var target = context.AssignmentTarget;
                if( !NamedValues.TryGetValue( target.Name, out Alloca varSlot ) )
                {
                    throw new CodeGeneratorException( $"Unknown variable name {target.Name}" );
                }

                lhs = varSlot;
            }
            else
            {
                lhs = context.primaryExpression( ).Accept( this );
            }

            foreach( var (op, rhs) in context.OperatorExpressions )
            {
                lhs = EmitBinaryOperator( lhs, op, rhs );
            }

            return lhs;
        }
        // </VisitExpression>

        // <VisitConditionalExpression>
        public override Value VisitConditionalExpression( [NotNull] ConditionalExpressionContext context )
        {
            var result = CreateEntryBlockAlloca( InstructionBuilder.InsertBlock.ContainingFunction, "ifresult.alloca" );

            var condition = context.Condition.Accept( this );
            if( condition == null )
            {
                return null;
            }

            var condBool = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, condition, Context.CreateConstant( 0.0 ) )
                                             .RegisterName( "ifcond" );

            var function = InstructionBuilder.InsertBlock.ContainingFunction;

            var thenBlock = Context.CreateBasicBlock( "then", function );
            var elseBlock = Context.CreateBasicBlock( "else" );
            var continueBlock = Context.CreateBasicBlock( "ifcont" );
            InstructionBuilder.Branch( condBool, thenBlock, elseBlock );

            // generate then block
            InstructionBuilder.PositionAtEnd( thenBlock );
            var thenValue = context.ThenExpression.Accept( this );
            if( thenValue == null )
            {
                return null;
            }

            InstructionBuilder.Store( thenValue, result );
            InstructionBuilder.Branch( continueBlock );

            // capture the insert in case generating else adds new blocks
            thenBlock = InstructionBuilder.InsertBlock;

            // generate else block
            function.BasicBlocks.Add( elseBlock );
            InstructionBuilder.PositionAtEnd( elseBlock );
            var elseValue = context.ElseExpression.Accept( this );
            if( elseValue == null )
            {
                return null;
            }

            InstructionBuilder.Store( elseValue, result );
            InstructionBuilder.Branch( continueBlock );
            elseBlock = InstructionBuilder.InsertBlock;

            // generate continue block
            function.BasicBlocks.Add( continueBlock );
            InstructionBuilder.PositionAtEnd( continueBlock );
            return InstructionBuilder.Load( result )
                                     .RegisterName( "ifresult" );
        }
        // </VisitConditionalExpression>

        // <VisitForExpression>
        public override Value VisitForExpression( [NotNull] ForExpressionContext context )
        {
            var function = InstructionBuilder.InsertBlock.ContainingFunction;
            string varName = context.Initializer.Name;
            var allocaVar = CreateEntryBlockAlloca( function, varName );

            // Emit the start code first, without 'variable' in scope.
            Value startVal = null;
            if( context.Initializer.Value != null )
            {
                startVal = context.Initializer.Value.Accept( this );
                if( startVal == null )
                {
                    return null;
                }
            }
            else
            {
                startVal = Context.CreateConstant( 0.0 );
            }

            // store the value into allocated location
            InstructionBuilder.Store( startVal, allocaVar );

            // Make the new basic block for the loop header, inserting after current
            // block.
            var preHeaderBlock = InstructionBuilder.InsertBlock;
            var loopBlock = Context.CreateBasicBlock( "loop", function );

            // Insert an explicit fall through from the current block to the loopBlock.
            InstructionBuilder.Branch( loopBlock );

            // Start insertion in loopBlock.
            InstructionBuilder.PositionAtEnd( loopBlock );

            // Within the loop, the variable is defined equal to the PHI node.
            // So, push a new scope for it and any values the body might set
            using( NamedValues.EnterScope( ) )
            {
                NamedValues[ varName ] = allocaVar;

                // Emit the body of the loop.  This, like any other expr, can change the
                // current BB.  Note that we ignore the value computed by the body, but don't
                // allow an error.
                if( context.BodyExpression.Accept( this ) == null )
                {
                    return null;
                }

                Value stepValue = Context.CreateConstant( 1.0 );

                if( context.StepExpression != null )
                {
                    stepValue = context.StepExpression.Accept( this );
                    if( stepValue == null )
                    {
                        return null;
                    }
                }

                // Compute the end condition.
                Value endCondition = context.EndExpression.Accept( this );
                if( endCondition == null )
                {
                    return null;
                }

                var curVar = InstructionBuilder.Load( allocaVar )
                                               .RegisterName( varName );
                var nextVar = InstructionBuilder.FAdd( curVar, stepValue )
                                                .RegisterName( "nextvar" );
                InstructionBuilder.Store( nextVar, allocaVar );

                // Convert condition to a bool by comparing non-equal to 0.0.
                endCondition = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, endCondition, Context.CreateConstant( 0.0 ) )
                                                 .RegisterName( "loopcond" );

                // Create the "after loop" block and insert it.
                var loopEndBlock = InstructionBuilder.InsertBlock;
                var afterBlock = Context.CreateBasicBlock( "afterloop", function );

                // Insert the conditional branch into the end of LoopEndBB.
                InstructionBuilder.Branch( endCondition, loopBlock, afterBlock );
                InstructionBuilder.PositionAtEnd( afterBlock );

                // for expression always returns 0.0 for consistency, there is no 'void'
                return Context.DoubleType.GetNullValue( );
            }
        }
        // </VisitForExpression>

        // <VisitUserOperators>
        public override Value VisitUnaryOpExpression( [NotNull] UnaryOpExpressionContext context )
        {
            // verify the operator was previously defined
            var opKind = RuntimeState.GetUnaryOperatorInfo( context.Op ).Kind;
            if( opKind == OperatorKind.None )
            {
                throw new CodeGeneratorException( $"invalid unary operator {context.Op}" );
            }

            string calleeName = UnaryPrototypeContext.GetOperatorFunctionName( context.OpToken );
            var function = FindCallTarget( calleeName );
            if( function == null )
            {
                throw new CodeGeneratorException( $"Unknown function reference {calleeName}" );
            }

            var arg = context.Rhs.Accept( this );
            return InstructionBuilder.Call( function, arg ).RegisterName( "calltmp" );
        }

        public override Value VisitBinaryPrototype( [NotNull] BinaryPrototypeContext context )
        {
            return GetOrDeclareFunction( new Prototype( context, context.Name ) );
        }

        public override Value VisitUnaryPrototype( [NotNull] UnaryPrototypeContext context )
        {
            return GetOrDeclareFunction( new Prototype( context, context.Name ) );
        }
        // </VisitUserOperators>

        // <VisitVarInExpression>
        public override Value VisitVarInExpression( [NotNull] VarInExpressionContext context )
        {
            using( NamedValues.EnterScope( ) )
            {
                Function function = InstructionBuilder.InsertBlock.ContainingFunction;
                foreach( var initializer in context.Initiaizers )
                {
                    Value initValue = Context.CreateConstant( 0.0 );
                    if( initializer.Value != null )
                    {
                        initValue = initializer.Value.Accept( this );
                    }

                    var alloca = CreateEntryBlockAlloca( function, initializer.Name );
                    InstructionBuilder.Store( initValue, alloca );
                    NamedValues[ initializer.Name ] = alloca;
                }

                return context.Scope.Accept( this );
            }
        }
        // </VisitVarInExpression>

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

            case ASSIGN:
                InstructionBuilder.Store( rhs, lhs );
                return rhs;

            // <EmitUserOperator>
            default:
                {
                    // User defined op?
                    var opKind = RuntimeState.GetBinOperatorInfo( op.Token.Type ).Kind;
                    if( opKind != OperatorKind.InfixLeftAssociative && opKind != OperatorKind.InfixRightAssociative )
                    {
                        throw new CodeGeneratorException( $"Invalid binary operator {op.Token.Text}" );
                    }

                    string calleeName = BinaryPrototypeContext.GetOperatorFunctionName( op.Token );
                    var function = FindCallTarget( calleeName );
                    if( function == null )
                    {
                        throw new CodeGeneratorException( $"Unknown function reference {calleeName}" );
                    }

                    return InstructionBuilder.Call( function, lhs, rhs ).RegisterName( "calltmp" );
                }
            // </EmitUserOperator>
            }
        }
        // </EmitBinaryOperator>

        // <InitializeModuleAndPassManager>
        private void InitializeModuleAndPassManager( )
        {
            Module = Context.CreateBitcodeModule( );
            Module.TargetTriple = TargetMachine.Triple;
            Module.Layout = TargetMachine.TargetData;
            FunctionPassManager = new FunctionPassManager( Module )
                                      .AddPromoteMemoryToRegisterPass( );

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
            // mark anonymous functions as always-inline and private so they can be inlined and then removed
            if( isAnonymous )
            {
                retVal.AddAttribute( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline )
                      .Linkage( Linkage.Private );
            }
            else
            {
                retVal.Linkage( Linkage.External );
            }

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
        private Function DefineFunction( Function function, ExpressionContext body )
        {
            if( !function.IsDeclaration )
            {
                throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
            }

            var basicBlock = function.AppendBasicBlock( "entry" );
            InstructionBuilder.PositionAtEnd( basicBlock );
            using( NamedValues.EnterScope( ) )
            {
                foreach( var arg in function.Parameters )
                {
                    var argSlot = CreateEntryBlockAlloca( function, arg.Name );
                    InstructionBuilder.Store( arg, argSlot );
                    NamedValues[ arg.Name ] = argSlot;
                }

                var funcReturn = body.Accept( this );
                if( funcReturn == null )
                {
                    function.EraseFromParent( );
                    return null;
                }

                InstructionBuilder.Return( funcReturn );
                function.Verify( );
            }

            FunctionPassManager.Run( function );

            return function;
        }
        // </DefineFunction>

        // <CreateEntryBlockAlloca>
        private static Alloca CreateEntryBlockAlloca( Function theFunction, string varName )
        {
            var tmpBldr = new InstructionBuilder( theFunction.EntryBlock );
            if( theFunction.EntryBlock.FirstInstruction != null )
            {
                tmpBldr.PositionBefore( theFunction.EntryBlock.FirstInstruction );
            }

            return tmpBldr.Alloca( theFunction.Context.DoubleType )
                          .RegisterName( varName );
        }
        // </CreateEntryBlockAlloca>

        // <PrivateMembers>
        private readonly bool DisableOptimizations;
        private readonly DynamicRuntimeState RuntimeState;
        private static int AnonNameIndex;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Alloca> NamedValues;
        private FunctionPassManager FunctionPassManager;
        private TargetMachine TargetMachine;
        private readonly PrototypeCollection FunctionPrototypes;
        private readonly List<Function> AnonymousFunctions;
        // </PrivateMembers>
    }
}
