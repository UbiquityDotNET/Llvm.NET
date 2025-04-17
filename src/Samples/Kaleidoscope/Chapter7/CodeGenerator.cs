// -----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.OrcJITv2;
using Ubiquity.NET.Llvm.Values;
using Ubiquity.NET.Runtime.Utils;

using ConstantExpression = Kaleidoscope.Grammar.AST.ConstantExpression;

namespace Kaleidoscope.Chapter7
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : KaleidoscopeAstVisitorBase<Value>
        , IDisposable
        , ICodeGenerator<Value>
    {
        #region Initialization
        public CodeGenerator(DynamicRuntimeState globalState, TextWriter? outputWriter = null)
            : base( null )
        {
            ArgumentNullException.ThrowIfNull( globalState );

            // set the global output writer for KlsJIT execution
            // the "built-in" functions need this to generate output somewhere.
            KaleidoscopeJIT.OutputWriter = outputWriter ?? Console.Out;
            if(globalState.LanguageLevel > LanguageLevel.MutableVariables)
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof( globalState ) );
            }

            RuntimeState = globalState;
            ThreadSafeContext = new();
            InstructionBuilder = new InstructionBuilder( ThreadSafeContext.PerThreadContext );
        }
        #endregion

        public void Dispose()
        {
            foreach(var tracker in FunctionModuleMap.Values)
            {
                tracker.Dispose();
            }

            KlsJIT.Dispose();
            Module?.Dispose();
            InstructionBuilder.Dispose();
            ThreadSafeContext.Dispose();
        }

        public Value? Generate(IAstNode ast)
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

            IContext ctx = ThreadSafeContext.PerThreadContext;
            InstructionBuilder?.Dispose();
            InstructionBuilder = new InstructionBuilder( ThreadSafeContext.PerThreadContext );
            Module?.Dispose();
            Module = ctx.CreateBitcodeModule();
            Debug.Assert( Module is not null, "Module initialization failed" );

            var function = definition.Accept( this ) as Function ?? throw new CodeGeneratorException(ExpectValidFunc);
            if(!function.ParentModule.Verify(out string msg))
            {
                throw new CodeGeneratorException(msg);
            }

            if(definition.IsAnonymous)
            {
                // Directly track modules for anonymous functions as calling the function is the guaranteed
                // next step and then it is removed as nothing can reference it again.
                // NOTE, this could eagerly compile the IR to an object file as a memory buffer and then add
                // that - but what would be the point? The JIT can do that for us as soon as the symbol is looked
                // up. The object support is more for existing object files than for generated IR.
                using ResourceTracker resourceTracker = KlsJIT.AddWithTracking(ThreadSafeContext, Module);
                Value retVal;

                // Invoking the function via a function pointer is an "unsafe" operation.
                // Also note that .NET has no mechanism to catch native exceptions like
                // access violations or stack overflows from infinite recursion. They will
                // crash the app.
                unsafe
                {
                    var pFunc = (delegate* unmanaged[Cdecl]<double>)KlsJIT.Lookup(definition.Name);
                    retVal = ctx.CreateConstant( pFunc() );
                    resourceTracker.RemoveAll();
                    return retVal;
                }
            }
            else
            {
                // Destroy any previously generated module for this function.
                // This allows re-definition as the new module will provide the
                // implementation.
                if(FunctionModuleMap.Remove( definition.Name, out ResourceTracker? tracker ))
                {
                    tracker.RemoveAll();
                    tracker.Dispose();
                }

                // Unknown if any future input will call the function so add it for lazy compilation.
                // Native code is generated for the module automatically only when required.
                FunctionModuleMap.Add( definition.Name, KlsJIT.AddWithTracking( ThreadSafeContext, Module ) );
                return function;
            }
        }

        public override Value? Visit(ConstantExpression constant)
        {
            ArgumentNullException.ThrowIfNull( constant );

            return ThreadSafeContext.PerThreadContext.CreateConstant( constant.Value );
        }

        #region BinaryOperatorExpression
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

            case BuiltInOperatorKind.Assign:
            {
                Alloca target = LookupVariable( ( ( VariableReferenceExpression )binaryOperator.Left ).Name );
                Value value = binaryOperator.Right.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr );
                InstructionBuilder.Store( value, target );
                return value;
            }

            default:
                throw new CodeGeneratorException( $"ICE: Invalid binary operator {binaryOperator.Op}" );
            }
        }
        #endregion

        public override Value? Visit(FunctionCallExpression functionCall)
        {
            ArgumentNullException.ThrowIfNull( functionCall );
            Debug.Assert(InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already");

            if(Module is null)
            {
                throw new InvalidOperationException( "Can't visit a function call without an active module" );
            }

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
            Debug.Assert(InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already");

            var function = GetOrDeclareFunction( definition.Signature );
            if(!function.IsDeclaration)
            {
                throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
            }

            try
            {
                var entryBlock = function.AppendBasicBlock( "entry" );
                InstructionBuilder.PositionAtEnd( entryBlock );
                using(NamedValues.EnterScope())
                {
                    foreach(var param in definition.Signature.Parameters)
                    {
                        var argSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                        .RegisterName( param.Name );
                        InstructionBuilder.Store( function.Parameters[ param.Index ], argSlot );
                        NamedValues[ param.Name ] = argSlot;
                    }

                    foreach(LocalVariableDeclaration local in definition.LocalVariables)
                    {
                        var localSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                          .RegisterName( local.Name );
                        NamedValues[ local.Name ] = localSlot;
                    }

                    EmitBranchToNewBlock( "body" );

                    var funcReturn = definition.Body.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidFunc );
                    InstructionBuilder.Return( funcReturn );
                    function.Verify();

                    return function;
                }
            }
            catch(CodeGeneratorException)
            {
                function.EraseFromParent();
                throw;
            }
        }
        #endregion

        #region VariableReferenceExpression
        public override Value? Visit(VariableReferenceExpression reference)
        {
            ArgumentNullException.ThrowIfNull( reference );

            var value = LookupVariable( reference.Name );

            // since the Alloca is created as a non-opaque pointer it is OK to just use the
            // ElementType. If full opaque pointer support was used, then the Lookup map
            // would need to include the type of the value allocated.
            return InstructionBuilder.Load( value.ElementType, value )
                                     .RegisterName( reference.Name );
        }
        #endregion

        #region ConditionalExpression
        public override Value? Visit(ConditionalExpression conditionalExpression)
        {
            ArgumentNullException.ThrowIfNull( conditionalExpression );
            Debug.Assert(InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already");

            var result = LookupVariable( conditionalExpression.ResultVariable.Name );

            var condition = conditionalExpression.Condition.Accept( this );
            if(condition == null)
            {
                return null;
            }

            var condBool = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, condition, ThreadSafeContext.PerThreadContext.CreateConstant( 0.0 ) )
                                             .RegisterName( "ifcond" );

            var function = InstructionBuilder.InsertFunction ?? throw new InternalCodeGeneratorException( "ICE: expected block that is attached to a function at this point" );

            var thenBlock = function.AppendBasicBlock( "then" );
            var elseBlock = function.AppendBasicBlock( "else" );
            var continueBlock = function.AppendBasicBlock( "ifcont" );
            InstructionBuilder.Branch( condBool, thenBlock, elseBlock );

            // generate then block instructions
            InstructionBuilder.PositionAtEnd( thenBlock );

            // InstructionBuilder.InserBlock after this point is !null
            Debug.Assert( InstructionBuilder.InsertBlock != null, "expected non-null InsertBlock" );
            var thenValue = conditionalExpression.ThenExpression.Accept( this );
            if(thenValue == null)
            {
                return null;
            }

            InstructionBuilder.Store( thenValue, result );
            InstructionBuilder.Branch( continueBlock );

            // generate else block
            InstructionBuilder.PositionAtEnd( elseBlock );
            var elseValue = conditionalExpression.ElseExpression.Accept( this );
            if(elseValue == null)
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
        public override Value? Visit(ForInExpression forInExpression)
        {
            ArgumentNullException.ThrowIfNull( forInExpression );
            Debug.Assert(InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already");

            var function = InstructionBuilder.InsertFunction ?? throw new InternalCodeGeneratorException( "ICE: Expected block attached to a function at this point" );

            string varName = forInExpression.LoopVariable.Name;
            IContext ctx = ThreadSafeContext.PerThreadContext;
            Alloca allocaVar = LookupVariable( varName );

            // Emit the start code first, without 'variable' in scope.
            Value? startVal;
            if(forInExpression.LoopVariable.Initializer != null)
            {
                startVal = forInExpression.LoopVariable.Initializer.Accept( this );
                if(startVal is null)
                {
                    return null;
                }
            }
            else
            {
                startVal = ctx.CreateConstant( 0.0 );
            }

            Debug.Assert( InstructionBuilder.InsertBlock != null, "expected non-null InsertBlock" );

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
            using(NamedValues.EnterScope())
            {
                EmitBranchToNewBlock( "ForInScope" );

                // Emit the body of the loop.  This, like any other expression, can change the
                // current BB.  Note that we ignore the value computed by the body, but don't
                // allow an error.
                if(forInExpression.Body.Accept( this ) == null)
                {
                    return null;
                }

                Value? stepValue = forInExpression.Step.Accept( this );
                if(stepValue == null)
                {
                    return null;
                }

                // Compute the end condition.
                Value? endCondition = forInExpression.Condition.Accept( this );
                if(endCondition == null)
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
                endCondition = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, endCondition, ctx.CreateConstant( 0.0 ) )
                                                 .RegisterName( "loopcond" );

                // Create the "after loop" block and insert it.
                var afterBlock = function.AppendBasicBlock( "afterloop" );

                // Insert the conditional branch into the end of LoopEndBB.
                InstructionBuilder.Branch( endCondition, loopBlock, afterBlock );
                InstructionBuilder.PositionAtEnd( afterBlock );

                // for expression always returns 0.0 for consistency, there is no 'void'
                return ctx.DoubleType.GetNullValue();
            }
        }
        #endregion

        #region VarInExpression
        public override Value? Visit( VarInExpression varInExpression )
        {
            ArgumentNullException.ThrowIfNull(varInExpression);

            IContext ctx = ThreadSafeContext.PerThreadContext;
            using(NamedValues.EnterScope())
            {
                EmitBranchToNewBlock( "VarInScope" );
                foreach(var localVar in varInExpression.LocalVariables)
                {
                    Alloca alloca = LookupVariable( localVar.Name );
                    Value initValue = ctx.CreateConstant( 0.0 );
                    if(localVar.Initializer != null)
                    {
                        initValue = localVar.Initializer.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidExpr );
                    }

                    InstructionBuilder.Store( initValue, alloca );
                }

                return varInExpression.Body.Accept( this );
            }
        }
        #endregion

        #region LookupVariable
        private Alloca LookupVariable( string name )
        {
            if(!NamedValues.TryGetValue( name, out Alloca? value ))
            {
                // Source input is validated by the parser and AstBuilder, therefore
                // this is the result of an internal error in the generator rather
                // then some sort of user error.
                throw new CodeGeneratorException( $"ICE: Unknown variable name: {name}" );
            }

            return value;
        }
        #endregion

        private void EmitBranchToNewBlock( string blockName )
        {
            var newBlock = InstructionBuilder.InsertFunction?.AppendBasicBlock( blockName )
                           ?? throw new InternalCodeGeneratorException("ICE: Expected an insertion block attached to a function at this point" );

            InstructionBuilder.Branch( newBlock );
            InstructionBuilder.PositionAtEnd( newBlock );
        }

        // Retrieves a Function for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private Function GetOrDeclareFunction(Prototype prototype)
        {
            if(Module is null)
            {
                throw new InvalidOperationException( "ICE: Can't get or declare a function without an active module" );
            }

            if(Module.TryGetFunction( prototype.Name, out Function? function ))
            {
                return function;
            }

            IContext ctx = ThreadSafeContext.PerThreadContext;
            var llvmSignature = ctx.GetFunctionType( returnType: ctx.DoubleType, args: prototype.Parameters.Select( _ => ctx.DoubleType ) );
            var retVal = Module.CreateFunction( prototype.Name, llvmSignature );

            int index = 0;
            foreach(var argId in prototype.Parameters)
            {
                retVal.Parameters[ index ].Name = argId.Name;
                ++index;
            }

            return retVal;
        }

        private const string ExpectValidExpr = "Expected a valid expression";
        private const string ExpectValidFunc = "Expected a valid function";

        #region PrivateMembers
        private Module? Module;
        private readonly DynamicRuntimeState RuntimeState;
        private readonly ThreadSafeContext ThreadSafeContext;
        private InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Alloca> NamedValues = new( );
        private readonly KaleidoscopeJIT KlsJIT = new( );
        private readonly Dictionary<string, ResourceTracker> FunctionModuleMap = [];
        #endregion
    }
}
