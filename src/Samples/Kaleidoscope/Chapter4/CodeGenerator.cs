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

namespace Kaleidoscope.Chapter4
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : KaleidoscopeAstVisitorBase<Value>
        , IDisposable
        , ICodeGenerator<Value>
    {
        #region Initialization
        public CodeGenerator( DynamicRuntimeState globalState, TextWriter? outputWriter = null )
            : base( null )
        {
            ArgumentNullException.ThrowIfNull( globalState );

            // set the global output writer for KlsJIT execution
            // the "built-in" functions need this to generate output somewhere.
            KaleidoscopeJIT.OutputWriter = outputWriter ?? Console.Out;
            if(globalState.LanguageLevel > LanguageLevel.SimpleExpressions)
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof( globalState ) );
            }

            RuntimeState = globalState;
            ThreadSafeContext = new();
        }
        #endregion

        #region Dispose
        public void Dispose( )
        {
            foreach(var tracker in FunctionModuleMap.Values)
            {
                tracker.Dispose();
            }

            KlsJIT.Dispose();
            Module?.Dispose();
            InstructionBuilder?.Dispose();
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

            var function = definition.Accept( this ) as Function ?? throw new CodeGeneratorException(ExpectValidFunc);
            if(!function.ParentModule.Verify( out string msg ))
            {
                throw new CodeGeneratorException( msg );
            }

            if(definition.IsAnonymous)
            {
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
        #endregion

        #region ConstantExpression
        public override Value? Visit( ConstantExpression constant )
        {
            ArgumentNullException.ThrowIfNull( constant );

            return ThreadSafeContext.PerThreadContext.CreateConstant( constant.Value );
        }
        #endregion

        public override Value? Visit( BinaryOperatorExpression binaryOperator )
        {
            ArgumentNullException.ThrowIfNull( binaryOperator );

            Debug.Assert( InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already" );
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

        #region FunctionCallExpression
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
        #endregion

        #region FunctionDefinition
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
                NamedValues.Clear();
                foreach(var param in definition.Signature.Parameters)
                {
                    NamedValues[ param.Name ] = function.Parameters[ param.Index ];
                }

                var funcReturn = definition.Body.Accept( this ) ?? throw new CodeGeneratorException( ExpectValidFunc );
                InstructionBuilder.Return( funcReturn );
                function.Verify();

                return function;
            }
            catch(CodeGeneratorException)
            {
                function.EraseFromParent();
                throw;
            }
        }
        #endregion

        public override Value? Visit( VariableReferenceExpression reference )
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
        #endregion

        private const string ExpectValidExpr = "Expected a valid expression";
        private const string ExpectValidFunc = "Expected a valid function";

        #region PrivateMembers
        private Module? Module;
        private readonly DynamicRuntimeState RuntimeState;
        private readonly ThreadSafeContext ThreadSafeContext;
        private InstructionBuilder? InstructionBuilder;
        private readonly Dictionary<string, Value> NamedValues = [];
        private readonly KaleidoscopeJIT KlsJIT = new( );
        private readonly Dictionary<string, ResourceTracker> FunctionModuleMap = [];
        #endregion
    }
}
