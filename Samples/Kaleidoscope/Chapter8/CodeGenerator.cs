﻿// -----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Transforms;
using Ubiquity.NET.Llvm.Values;

using ConstantExpression = Kaleidoscope.Grammar.AST.ConstantExpression;

namespace Kaleidoscope.Chapter8
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : AstVisitorBase<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<BitcodeModule>
    {
        #region Initialization
        public CodeGenerator( DynamicRuntimeState globalState, TargetMachine machine, bool disableOptimization = false )
            : base( null )
        {
            globalState.ValidateNotNull( nameof( globalState ) );
            machine.ValidateNotNull( nameof( machine ) );

            if( globalState.LanguageLevel > LanguageLevel.MutableVariables )
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof( globalState ) );
            }

            RuntimeState = globalState;
            Context = new Context( );
            TargetMachine = machine;
            DisableOptimizations = disableOptimization;
            InstructionBuilder = new InstructionBuilder( Context );
            Module = Context.CreateBitcodeModule( );
            Module.TargetTriple = machine.Triple;
            Module.Layout = TargetMachine.TargetData;
            FunctionPassManager = new FunctionPassManager( Module );
            FunctionPassManager.AddPromoteMemoryToRegisterPass( );

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

        #region Dispose
        public void Dispose( )
        {
            Module.Dispose( );
            Context.Dispose( );
        }
        #endregion

        #region Generate
        public OptionalValue<BitcodeModule> Generate( IAstNode ast )
        {
            ast.ValidateNotNull( nameof( ast ) );
            ast.Accept( this );

            if( AnonymousFunctions.Count > 0 )
            {
                var mainFunction = Module.CreateFunction( "main", Context.GetFunctionType( Context.VoidType ) );
                var block = mainFunction.AppendBasicBlock( "entry" );
                var irBuilder = new InstructionBuilder( block );
                var printdFunc = Module.CreateFunction( "printd", Context.GetFunctionType( Context.DoubleType, Context.DoubleType ) );
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
                var mpm = new ModulePassManager( );
                mpm.AddAlwaysInlinerPass( )
                   .AddGlobalDCEPass( )
                   .Run( Module );
            }

            return OptionalValue.Create( Module );
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

            case BuiltInOperatorKind.Assign:
                Alloca target = LookupVariable( ( ( VariableReferenceExpression )binaryOperator.Left ).Name );
                Value value = binaryOperator.Right.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr );
                InstructionBuilder.Store( value, target );
                return value;

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
            if( RuntimeState.FunctionDeclarations.TryGetValue( targetName, out Prototype? target ) )
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
                using( NamedValues.EnterScope( ) )
                {
                    foreach( var param in definition.Signature.Parameters )
                    {
                        var argSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                        .RegisterName( param.Name );
                        InstructionBuilder.Store( function.Parameters[ param.Index ], argSlot );
                        NamedValues[ param.Name ] = argSlot;
                    }

                    foreach( LocalVariableDeclaration local in definition.LocalVariables )
                    {
                        var localSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                          .RegisterName( local.Name );
                        NamedValues[ local.Name ] = localSlot;
                    }

                    EmitBranchToNewBlock( "body" );

                    var funcReturn = definition.Body.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidFunc );
                    InstructionBuilder.Return( funcReturn );
                    function.Verify( );

                    FunctionPassManager.Run( function );

                    if( definition.IsAnonymous )
                    {
                        function.AddAttribute( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline )
                                .Linkage( Linkage.Private );

                        AnonymousFunctions.Add( function );
                    }

                    return function;
                }
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
            var value = LookupVariable( reference.Name );

            // since the Alloca is created as a non-opaque pointer it is OK to just use the
            // ElementType. If full opaque pointer support was used, then the Lookup map
            // would need to include the type of the value allocated.
            return InstructionBuilder.Load( value.ElementType, value )
                                     .RegisterName( reference.Name );
        }
        #endregion

        #region ConditionalExpression
        public override Value? Visit( ConditionalExpression conditionalExpression )
        {
            conditionalExpression.ValidateNotNull( nameof( conditionalExpression ) );
            var result = LookupVariable( conditionalExpression.ResultVariable.Name );

            var condition = conditionalExpression.Condition.Accept( this );
            if( condition == null )
            {
                return null;
            }

            var condBool = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, condition, Context.CreateConstant( 0.0 ) )
                                             .RegisterName( "ifcond" );

            var function = InstructionBuilder.InsertFunction;
            if( function is null )
            {
                throw new InternalCodeGeneratorException( "ICE: expected block that is attached to a function at this point" );
            }

            var thenBlock = function.AppendBasicBlock( "then" );
            var elseBlock = function.AppendBasicBlock( "else" );
            var continueBlock = function.AppendBasicBlock( "ifcont" );
            InstructionBuilder.Branch( condBool, thenBlock, elseBlock );

            // generate then block instructions
            InstructionBuilder.PositionAtEnd( thenBlock );

            // InstructionBuilder.InserBlock after this point is !null
            Debug.Assert( InstructionBuilder.InsertBlock != null, "expected non-null InsertBlock" );
            var thenValue = conditionalExpression.ThenExpression.Accept( this );
            if( thenValue == null )
            {
                return null;
            }

            InstructionBuilder.Store( thenValue, result );
            InstructionBuilder.Branch( continueBlock );

            // generate else block
            InstructionBuilder.PositionAtEnd( elseBlock );
            var elseValue = conditionalExpression.ElseExpression.Accept( this );
            if( elseValue == null )
            {
                return null;
            }

            InstructionBuilder.Store( elseValue, result );
            InstructionBuilder.Branch( continueBlock );

            // generate continue block
            InstructionBuilder.PositionAtEnd( continueBlock );

            // since the Alloca is created as a non-opaque pointer it is OK to just use the
            // ElementType. If full opaque pointer support was used, then the Lookup map
            // would need to include the type of the value allocated.
            return InstructionBuilder.Load( result.ElementType, result )
                                     .RegisterName( "ifresult" );
        }
        #endregion

        #region ForInExpression
        public override Value? Visit( ForInExpression forInExpression )
        {
            forInExpression.ValidateNotNull( nameof( forInExpression ) );
            var function = InstructionBuilder.InsertFunction;
            if( function is null )
            {
                throw new InternalCodeGeneratorException( "ICE: Expected block attached to a function at this point" );
            }

            string varName = forInExpression.LoopVariable.Name;
            Alloca allocaVar = LookupVariable( varName );

            // Emit the start code first, without 'variable' in scope.
            Value? startVal;
            if( forInExpression.LoopVariable.Initializer != null )
            {
                startVal = forInExpression.LoopVariable.Initializer.Accept( this );
                if( startVal is null )
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

            // Make the new basic block for the loop header.
            var loopBlock = function.AppendBasicBlock( "loop" );

            // Insert an explicit fall through from the current block to the loopBlock.
            InstructionBuilder.Branch( loopBlock );

            // Start insertion in loopBlock.
            InstructionBuilder.PositionAtEnd( loopBlock );

            // Within the loop, the variable is defined equal to the PHI node.
            // So, push a new scope for it and any values the body might set
            using( NamedValues.EnterScope( ) )
            {
                EmitBranchToNewBlock( "ForInScope" );

                // Emit the body of the loop.  This, like any other expression, can change the
                // current BB.  Note that we ignore the value computed by the body, but don't
                // allow an error.
                if( forInExpression.Body.Accept( this ) == null )
                {
                    return null;
                }

                Value? stepValue = forInExpression.Step.Accept( this );
                if( stepValue == null )
                {
                    return null;
                }

                // Compute the end condition.
                Value? endCondition = forInExpression.Condition.Accept( this );
                if( endCondition == null )
                {
                    return null;
                }

                // since the Alloca is created as a non-opaque pointer it is OK to just use the
                // ElementType. If full opaque pointer support was used, then the Lookup map
                // would need to include the type of the value allocated.
                var curVar = InstructionBuilder.Load( allocaVar.ElementType, allocaVar )
                                               .RegisterName( varName );
                var nextVar = InstructionBuilder.FAdd( curVar, stepValue )
                                                .RegisterName( "nextvar" );
                InstructionBuilder.Store( nextVar, allocaVar );

                // Convert condition to a bool by comparing non-equal to 0.0.
                endCondition = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, endCondition, Context.CreateConstant( 0.0 ) )
                                                 .RegisterName( "loopcond" );

                // Create the "after loop" block and insert it.
                var afterBlock = function.AppendBasicBlock( "afterloop" );

                // Insert the conditional branch into the end of LoopEndBB.
                InstructionBuilder.Branch( endCondition, loopBlock, afterBlock );
                InstructionBuilder.PositionAtEnd( afterBlock );

                // for expression always returns 0.0 for consistency, there is no 'void'
                return Context.DoubleType.GetNullValue( );
            }
        }
        #endregion

        #region VarInExpression
        public override Value? Visit( VarInExpression varInExpression )
        {
            varInExpression.ValidateNotNull( nameof( varInExpression ) );
            using( NamedValues.EnterScope( ) )
            {
                EmitBranchToNewBlock( "VarInScope" );
                foreach( var localVar in varInExpression.LocalVariables )
                {
                    Alloca alloca = LookupVariable( localVar.Name );
                    Value initValue = Context.CreateConstant( 0.0 );
                    if( localVar.Initializer != null )
                    {
                        initValue = localVar.Initializer.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr );
                    }

                    InstructionBuilder.Store( initValue, alloca );
                }

                return varInExpression.Body.Accept( this );
            }
        }
        #endregion

        private Alloca LookupVariable( string name )
        {
            if( !NamedValues.TryGetValue( name, out Alloca? value ) )
            {
                // Source input is validated by the parser and AstBuilder, therefore
                // this is the result of an internal error in the generator rather
                // then some sort of user error.
                throw new CodeGeneratorException( $"ICE: Unknown variable name: {name}" );
            }

            return value;
        }

        private void EmitBranchToNewBlock( string blockName )
        {
            var newBlock = InstructionBuilder.InsertFunction?.AppendBasicBlock( blockName )
                           ?? throw new InternalCodeGeneratorException("ICE: Expected an insertion block attached to a function at this point" );
            InstructionBuilder.Branch( newBlock );
            InstructionBuilder.PositionAtEnd( newBlock );
        }

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
            var retVal = Module.CreateFunction( prototype.Name, llvmSignature );

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
        private readonly BitcodeModule Module;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Alloca> NamedValues = new( );
        private readonly FunctionPassManager FunctionPassManager;
        private readonly bool DisableOptimizations;
        private readonly TargetMachine TargetMachine;
        private readonly List<IrFunction> AnonymousFunctions = new( );
        #endregion
    }
}
