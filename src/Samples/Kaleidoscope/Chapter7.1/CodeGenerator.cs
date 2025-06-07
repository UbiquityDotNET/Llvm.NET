// -----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.OrcJITv2;
using Ubiquity.NET.Llvm.Values;
using Ubiquity.NET.Runtime.Utils;

using ConstantExpression = Kaleidoscope.Grammar.AST.ConstantExpression;

namespace Kaleidoscope.Chapter71
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : KaleidoscopeAstVisitorBase<Value>
        , IDisposable
        , ICodeGenerator<Value>
    {
        public CodeGenerator( DynamicRuntimeState globalState, TextWriter? outputWriter = null )
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

            // set up support needed for lazy compilation
            string triple = KlsJIT.TripleString;
            JitISM = new LocalIndirectStubsManager( triple );
            JitLCTM = KlsJIT.Session.CreateLazyCallThroughManager( triple );
        }

        #region Dispose
        public void Dispose( )
        {
            // NOTE: There is no map of resource trackers as the JIT handles
            // calling Destroy on a materializer to release any resources it
            // might own.
            JitLCTM.Dispose();
            JitISM.Dispose();
            KlsJIT.Dispose();
            Module?.Dispose();
            InstructionBuilder.Dispose();
            ThreadSafeContext.Dispose();
        }
        #endregion

        #region Generate
        public Value? Generate( IAstNode ast )
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

            if(definition.IsAnonymous)
            {
                // Generate the LLVM IR for this function into the module
                _ = definition.Accept( this ) as Function ?? throw new CodeGeneratorException( ExpectValidFunc );

                // Directly track modules for anonymous functions as calling the function is the guaranteed
                // next step and then it is removed as nothing can reference it again.
                // NOTE, this could eagerly compile the IR to an object file as a memory buffer and then add
                // that - but what would be the point? The JIT can do that for us as soon as the symbol is looked
                // up. The object support is more for existing object files than for generated IR.
                using ResourceTracker resourceTracker = KlsJIT.AddWithTracking(ThreadSafeContext, Module);

                // Invoking the function via a function pointer is an "unsafe" operation.
                // Also note that .NET has no mechanism to catch native exceptions like
                // access violations or stack overflows from infinite recursion. They will
                // crash the app.
                double nativeRetVal;
                unsafe
                {
                    var pFunc = (delegate* unmanaged[Cdecl]<double>)KlsJIT.Lookup(definition.Name);
                    nativeRetVal = pFunc();
                }

                Value retVal = ctx.CreateConstant( nativeRetVal );
                resourceTracker.RemoveAll();
                return retVal;
            }
            else
            {
                // It is unknown if any future input will call the function so don't even generate IR
                // until it is needed. JIT triggers the callback to 'Materialize' the IR module when
                // the symbol is looked up so the JIT can then generate native code only when required.
                AddLazyMaterializer( definition );
                return default;
            }
        }
        #endregion

        public override Value? Visit( ConstantExpression constant )
        {
            ArgumentNullException.ThrowIfNull( constant );

            return ThreadSafeContext.PerThreadContext.CreateConstant( constant.Value );
        }

        public override Value? Visit( BinaryOperatorExpression binaryOperator )
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

        public override Value? Visit( FunctionCallExpression functionCall )
        {
            ArgumentNullException.ThrowIfNull( functionCall );
            Debug.Assert( InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already" );

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

        public override Value? Visit( FunctionDefinition definition )
        {
            ArgumentNullException.ThrowIfNull( definition );
            Debug.Assert( InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already" );

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

        public override Value? Visit( VariableReferenceExpression reference )
        {
            ArgumentNullException.ThrowIfNull( reference );

            var value = LookupVariable( reference.Name );

            // since the alloca is created as a non-opaque pointer it is OK to just use the
            // ElementType. If full opaque pointer support was used, then the Lookup map
            // would need to include the type of the value allocated.
            return InstructionBuilder.Load( value.ElementType, value )
                                     .RegisterName( reference.Name );
        }

        public override Value? Visit( ConditionalExpression conditionalExpression )
        {
            ArgumentNullException.ThrowIfNull( conditionalExpression );
            Debug.Assert( InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already" );

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

            // InstructionBuilder.InsertBlock after this point is !null
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

        public override Value? Visit( ForInExpression forInExpression )
        {
            ArgumentNullException.ThrowIfNull( forInExpression );
            Debug.Assert( InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already" );

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

        public override Value? Visit( VarInExpression varInExpression )
        {
            ArgumentNullException.ThrowIfNull( varInExpression );

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

        private void EmitBranchToNewBlock( string blockName )
        {
            var newBlock = InstructionBuilder.InsertFunction?.AppendBasicBlock( blockName )
                           ?? throw new InternalCodeGeneratorException("ICE: Expected an insertion block attached to a function at this point" );

            InstructionBuilder.Branch( newBlock );
            InstructionBuilder.PositionAtEnd( newBlock );
        }

        // Retrieves a Function for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private Function GetOrDeclareFunction( Prototype prototype )
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

        #region AddLazyMaterializer
        private void AddLazyMaterializer( FunctionDefinition definition )
        {
            FunctionDefinition implDefinition = CloneAndRenameFunction( definition );

            var dyLib = KlsJIT.MainLib;
            using var mangledName = KlsJIT.MangleAndIntern(definition.Name);
            using var mangledBodyName = KlsJIT.MangleAndIntern(implDefinition.Name);
            var commonSymbolFlags = new SymbolFlags(SymbolGenericOption.Exported | SymbolGenericOption.Callable);

            var symbols = new KvpArrayBuilder<SymbolStringPoolEntry, SymbolFlags>
            {
                [mangledBodyName] = commonSymbolFlags,
            }.ToImmutable();

            using var materializer = new CustomMaterializationUnit($"{definition.Name}MU", Materialize, symbols);
            dyLib.Define( materializer );

            var reexports = new KvpArrayBuilder<SymbolStringPoolEntry, SymbolAliasMapEntry>
            {
                [mangledName] = new(mangledBodyName, commonSymbolFlags)
            }.ToImmutable();

            using var lazyReExports = new LazyReExportsMaterializationUnit(JitLCTM, JitISM, dyLib, reexports);
            dyLib.Define( lazyReExports );
            return;

            // Local function to materialize the IR for the AST in implDefinition.
            // This is a local function to enable it to "capture" the AST and any
            // other values needed. The GC considers these "live" until either the
            // JIT is destroyed, the materializer is removed from the JIT, or the
            // symbol is looked up and the materializer runs.
            // NOTE: This function is called by the JIT asynchronously when the
            // symbol is resolved to an address in the JIT the first time. Thus,
            // it MUST not capture any IDisposable objects such as the mangled
            // symbol names as they are most likely already disposed by the time
            // this is called.
            void Materialize( MaterializationResponsibility r )
            {
                // symbol strings returned are NOT owned by this function so Dispose() isn't needed
                // (Though it is an allowed NOP that silences compiler/analyzer warnings)
                using var symbols = r.GetRequestedSymbols();
                Debug.Assert( symbols.Count == 1, "Unexpected number of symbols!" );

                using var mangledBodyName = KlsJIT.MangleAndIntern(implDefinition.Name);

                ThreadSafeModule tsm;
                if(symbols[ 0 ].Equals( mangledBodyName ))
                {
                    Debug.WriteLine( "Generating code for {0}", mangledBodyName );

                    Module?.Dispose();
                    Module = ThreadSafeContext.PerThreadContext.CreateBitcodeModule();
                    try
                    {
                        // generate a function from the AST into the module
                        _ = implDefinition.Accept( this ) ?? throw new CodeGeneratorException( "Failed to lazy generate function - this is an application crash scenario" );
                        tsm = new( ThreadSafeContext, Module );
                    }
                    finally
                    {
                        Module?.Dispose();
                        Module = null;
                    }
                }
                else
                {
                    Debug.WriteLine( "Unknown symbol" );

                    // Not a known symbol - fail the materialization request.
                    r.Fail();
                    r.Dispose();
                    return;
                }

                // In case of an exception clean up the created ThreadSafeModule instance.
                // Dispose is a NOP once transferred into Native code
                using(tsm)
                {
                    // Finally emit the module to the JIT.
                    // This transfers ownership of both the responsibility AND the module
                    // to the native LLVM JIT. The JIT will perform any additional transforms
                    // that are registered (for KLS that includes setting the data layout
                    // and running optimization passes)
                    KlsJIT.TransformLayer.Emit( r, tsm );
                }
            }
        }
        #endregion

        private const string ExpectValidExpr = "Expected a valid expression";
        private const string ExpectValidFunc = "Expected a valid function";

        #region CloneAndRenameFunction
        [SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "Truly lazy JIT functionality for Windows is disabled for now..." )]
        private static FunctionDefinition CloneAndRenameFunction( FunctionDefinition definition )
        {
            // clone the definition with a new name, note that this is really
            // a shallow clone so there's minimal overhead for the cloning.
            var newSignature = new Prototype( definition.Signature.Location
                                            , definition.Signature.Name + "$impl"
                                            , definition.Signature.Parameters
                                            );

            var implDefinition = new FunctionDefinition( definition.Location
                                                       , newSignature
                                                       , definition.Body
                                                       , definition.LocalVariables.ToImmutableArray( )
                                                       );
            return implDefinition;
        }
        #endregion

        #region PrivateMembers
        private Module? Module;
        private readonly DynamicRuntimeState RuntimeState;
        private readonly ThreadSafeContext ThreadSafeContext;
        private InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Alloca> NamedValues = new( );
        private readonly KaleidoscopeJIT KlsJIT = new( );
        private readonly LocalIndirectStubsManager JitISM;
        private readonly LazyCallThroughManager JitLCTM;
        #endregion
    }
}
