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
using Ubiquity.NET.Llvm.JIT.OrcJITv2;
using Ubiquity.NET.Llvm.Values;

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
        public CodeGenerator(DynamicRuntimeState globalState, TextWriter? outputWriter = null)
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
            InstructionBuilder = new InstructionBuilder( ThreadSafeContext.PerThreadContext );
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            KlsJIT.Dispose();
            Module?.Dispose();
            ThreadSafeContext.Dispose();
        }
        #endregion

        #region Generate
        public OptionalValue<Value> Generate(IAstNode ast)
        {
            ArgumentNullException.ThrowIfNull( ast );

            // Prototypes, including extern are ignored as AST generation
            // adds them to the RuntimeState so that already has the declarations
            if(ast is not FunctionDefinition definition)
            {
                return default;
            }

            Context ctx = ThreadSafeContext.PerThreadContext;
            Module = ctx.CreateBitcodeModule();
            Debug.Assert( Module is not null, "Module initialization failed" );

            var function = definition.Accept( this ) as Function ?? throw new CodeGeneratorException(ExpectValidFunc);

            if(definition.IsAnonymous)
            {
                // directly track modules for anonymous functions as calling the function is the guaranteed next step
                // and then it is removed as nothing an reference it again.
                using ResourceTracker resourceTracker = KlsJIT.Add(ThreadSafeContext, Module);
                Value retVal;

                // invoking the function is an "unsafe" operation via a function pointer
                unsafe
                {
                    try
                    {
                        var pFunc = (delegate* unmanaged[Cdecl]<double>)KlsJIT.Lookup(definition.Name);
                        retVal = ctx.CreateConstant( pFunc() );
                        resourceTracker.RemoveAll();
                        return OptionalValue.Create<Value>( retVal );
                    }
                    catch
                    {
                        // Console.Error.WriteLine(ex.Message);
                        return default;
                    }
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
                FunctionModuleMap.Add( definition.Name, KlsJIT.Add( ThreadSafeContext, Module ) );
                return OptionalValue.Create<Value>( function );
            }
        }
        #endregion

        #region ConstantExpression
        public override Value? Visit(ConstantExpression constant)
        {
            ArgumentNullException.ThrowIfNull( constant );

            return ThreadSafeContext.PerThreadContext.CreateConstant( constant.Value );
        }
        #endregion

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

        #region FunctionCallExpression
        public override Value? Visit(FunctionCallExpression functionCall)
        {
            ArgumentNullException.ThrowIfNull( functionCall );
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

                return function;
            }
            catch(CodeGeneratorException)
            {
                function.EraseFromParent();
                throw;
            }
        }

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
            if(Module is null)
            {
                throw new InvalidOperationException( "ICE: Can't get or declare a function without an active module" );
            }

            if(Module.TryGetFunction( prototype.Name, out Function? function ))
            {
                return function;
            }

            Context ctx = ThreadSafeContext.PerThreadContext;
            var llvmSignature = ctx.GetFunctionType( ctx.DoubleType, prototype.Parameters.Select( _ => ctx.DoubleType ) );
            var retVal = Module.CreateFunction( prototype.Name, llvmSignature );

            if(!prototype.IsExtern)
            {
                // Any function created by this generator from AST should NOT end up optimized into any built-in or other intrinsic.
                // LLVM has a bug (https://github.com/llvm/llvm-project/issues/130172) [:( Closed as 'Not planned']
                // that will think some things are valid runtime library calls it can optimize by substituting a
                // const value for the return instead of the actual returned value.
                // There is NO way to alter or customize the `TargetLibraryInfo` it is baked into the LLVM code
                // and depends on the triple so there's no way to control it. Thus, anything that comes from the
                // AST is considered as NOT built-in and should not be replaced.
                retVal.AddAttributes( FunctionAttributeIndex.Function, AttributeKind.NoBuiltIn );
            }

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
        private BitcodeModule? Module;
        private readonly DynamicRuntimeState RuntimeState;
        private readonly ThreadSafeContext ThreadSafeContext = new();
        private readonly InstructionBuilder InstructionBuilder;
        private readonly Dictionary<string, Value> NamedValues = [];
        private readonly KaleidoscopeJIT KlsJIT = new( );
        private readonly Dictionary<string, ResourceTracker> FunctionModuleMap = [];
        #endregion
    }
}
