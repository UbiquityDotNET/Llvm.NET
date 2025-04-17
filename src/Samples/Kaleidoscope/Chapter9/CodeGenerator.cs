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

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Values;
using Ubiquity.NET.Runtime.Utils;

using ConstantExpression = Kaleidoscope.Grammar.AST.ConstantExpression;

namespace Kaleidoscope.Chapter9
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : KaleidoscopeAstVisitorBase<Value, DIBuilder>
        , IDisposable
        , ICodeGenerator<Module>
    {
        #region Initialization
        public CodeGenerator( DynamicRuntimeState globalState, TargetMachine machine, string sourcePath)
            : base( null )
        {
            ArgumentNullException.ThrowIfNull( globalState );
            ArgumentNullException.ThrowIfNull( machine );

            if(globalState.LanguageLevel > LanguageLevel.MutableVariables)
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof( globalState ) );
            }

            RuntimeState = globalState;
            Context = new Context( );
            TargetMachine = machine;
            InstructionBuilder = new InstructionBuilder( Context );
            Module = Context.CreateBitcodeModule( Path.GetFileName( sourcePath ));
            Module.TargetTriple = machine.Triple;

            using var layout = TargetMachine.CreateTargetData();
            Module.Layout = layout;
            SourcePath = sourcePath;
        }
        #endregion

        #region Dispose
        public void Dispose( )
        {
            Module.Dispose( );
            InstructionBuilder.Dispose();
            Context.Dispose( );
        }
        #endregion

        #region Generate
        public Module? Generate( IAstNode ast )
        {
            ArgumentNullException.ThrowIfNull( ast );

            using var diBuilder = new DIBuilder(Module);
            var cu = diBuilder.CreateCompileUnit(SourceLanguage.C, SourcePath, "Kaleidoscope Compiler");

            Debug.Assert( cu != null, "Expected non null compile unit" );
            Debug.Assert( cu.File != null, "Expected non-null file for compile unit" );
            DoubleType = new DebugBasicType( Context.DoubleType, in diBuilder, "double", DiTypeKind.Float );

            // use this instance and the DIBuilder to visit the AST
            ast.Accept( this, in diBuilder );

            if( AnonymousFunctions.Count > 0 )
            {
                var mainFunction = Module.CreateFunction( "main", Context.GetFunctionType( Context.VoidType ) );
                var block = mainFunction.AppendBasicBlock( "entry" );
                using var irBuilder = new InstructionBuilder( block );
                var printdFunc = Module.CreateFunction( "printd", Context.GetFunctionType( Context.DoubleType, Context.DoubleType ) );
                foreach( var anonFunc in AnonymousFunctions )
                {
                    var value = irBuilder.Call( anonFunc );
                    irBuilder.Call( printdFunc, value );
                }

                irBuilder.Return( );
            }

            return Module;
        }
        #endregion

        #region ConstantExpression
        public override Value? Visit(ConstantExpression constant, ref readonly DIBuilder diBuilder)
        {
            ArgumentNullException.ThrowIfNull( constant );

            return Context.CreateConstant( constant.Value );
        }
        #endregion

        #region BinaryOperatorExpression
        public override Value? Visit(BinaryOperatorExpression binaryOperator, ref readonly DIBuilder diBuilder)
        {
            ArgumentNullException.ThrowIfNull( binaryOperator );
            EmitLocation( binaryOperator );

            switch(binaryOperator.Op)
            {
            case BuiltInOperatorKind.Less:
            {
                var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan
                                                    , binaryOperator.Left.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                                    , binaryOperator.Right.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                                    ).RegisterName( "cmptmp" );
                return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                         .RegisterName( "booltmp" );
            }

            case BuiltInOperatorKind.Pow:
            {
                var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ), in diBuilder );
                return InstructionBuilder.Call( pow
                                              , binaryOperator.Left.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "powtmp" );
            }

            case BuiltInOperatorKind.Add:
                return InstructionBuilder.FAdd( binaryOperator.Left.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "addtmp" );

            case BuiltInOperatorKind.Subtract:
                return InstructionBuilder.FSub( binaryOperator.Left.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "subtmp" );

            case BuiltInOperatorKind.Multiply:
                return InstructionBuilder.FMul( binaryOperator.Left.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "multmp" );

            case BuiltInOperatorKind.Divide:
                return InstructionBuilder.FDiv( binaryOperator.Left.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              , binaryOperator.Right.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr )
                                              ).RegisterName( "divtmp" );

            case BuiltInOperatorKind.Assign:
            {
                Alloca target = LookupVariable( ( ( VariableReferenceExpression )binaryOperator.Left ).Name );
                Value value = binaryOperator.Right.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr );
                InstructionBuilder.Store( value, target );
                return value;
            }

            default:
                throw new CodeGeneratorException( $"ICE: Invalid binary operator {binaryOperator.Op}" );
            }
        }
        #endregion

        #region FunctionCallExpression
        public override Value? Visit(FunctionCallExpression functionCall, ref readonly DIBuilder diBuilder)
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
                function = GetOrDeclareFunction( target, in diBuilder );
            }
            else if(!Module.TryGetFunction( targetName, out function ))
            {
                throw new CodeGeneratorException( $"Definition for function {targetName} not found" );
            }

            var args = new Value[functionCall.Arguments.Count];
            for(int i = 0; i < args.Length; ++i)
            {
                args[i] = functionCall.Arguments[i].Accept(this, in diBuilder) ?? throw new CodeGeneratorException(ExpectValidExpr);
            }

            EmitLocation( functionCall );
            return InstructionBuilder.Call( function, args ).RegisterName( "calltmp" );
        }
        #endregion

        #region FunctionDefinition
        public override Value? Visit(FunctionDefinition definition, ref readonly DIBuilder diBuilder)
        {
            ArgumentNullException.ThrowIfNull( definition );
            Debug.Assert(InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already");

            var function = GetOrDeclareFunction( definition.Signature, in diBuilder );
            if(!function.IsDeclaration)
            {
                throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
            }

            Debug.Assert( function.DISubProgram != null, "Expected function with non-null DISubProgram" );
            LexicalBlocks.Push( function.DISubProgram );
            try
            {
                var entryBlock = function.AppendBasicBlock( "entry" );
                InstructionBuilder.PositionAtEnd( entryBlock );

                // Unset the location for the prologue emission (leading instructions with no
                // location in a function are considered part of the prologue and the debugger
                // will run past them when breaking on a function)
                EmitLocation( null );

                using(NamedValues.EnterScope())
                {
                    foreach(var param in definition.Signature.Parameters)
                    {
                        var argSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                        .RegisterName( param.Name );
                        AddDebugInfoForAlloca( argSlot, function, param, in diBuilder );
                        InstructionBuilder.Store( function.Parameters[ param.Index ], argSlot );
                        NamedValues[ param.Name ] = argSlot;
                    }

                    foreach(LocalVariableDeclaration local in definition.LocalVariables)
                    {
                        var localSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                          .RegisterName( local.Name );
                        AddDebugInfoForAlloca( localSlot, function, local, in diBuilder );
                        NamedValues[ local.Name ] = localSlot;
                    }

                    EmitBranchToNewBlock( "body" );

                    var funcReturn = definition.Body.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidFunc );
                    InstructionBuilder.Return( funcReturn );
                    diBuilder.Finish( function.DISubProgram );
                    function.Verify();

                    if( definition.IsAnonymous )
                    {
                        function.AddAttribute( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline )
                                .Linkage( Linkage.Private );

                        AnonymousFunctions.Add( function );
                    }

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
        public override Value? Visit(VariableReferenceExpression reference, ref readonly DIBuilder diBuilder)
        {
            ArgumentNullException.ThrowIfNull( reference );

            var value = LookupVariable( reference.Name );

            EmitLocation( reference );

            // since the Alloca is created as a non-opaque pointer it is OK to just use the
            // ElementType. If full opaque pointer support was used, then the Lookup map
            // would need to include the type of the value allocated.
            return InstructionBuilder.Load( value.ElementType, value )
                                     .RegisterName( reference.Name );
        }
        #endregion

        #region ConditionalExpression
        public override Value? Visit( ConditionalExpression conditionalExpression, ref readonly DIBuilder diBuilder )
        {
            ArgumentNullException.ThrowIfNull( conditionalExpression );
            Debug.Assert(InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already");

            var result = LookupVariable( conditionalExpression.ResultVariable.Name );

            EmitLocation( conditionalExpression );
            var condition = conditionalExpression.Condition.Accept( this, in diBuilder );
            if(condition == null)
            {
                return null;
            }

            EmitLocation( conditionalExpression );

            var condBool = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, condition, Context.CreateConstant( 0.0 ) )
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
            var thenValue = conditionalExpression.ThenExpression.Accept( this, in diBuilder );
            if(thenValue == null)
            {
                return null;
            }

            InstructionBuilder.Store( thenValue, result );
            InstructionBuilder.Branch( continueBlock );

            // generate else block
            InstructionBuilder.PositionAtEnd( elseBlock );
            var elseValue = conditionalExpression.ElseExpression.Accept( this, in diBuilder );
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
        public override Value? Visit(ForInExpression forInExpression, ref readonly DIBuilder diBuilder)
        {
            ArgumentNullException.ThrowIfNull( forInExpression );
            Debug.Assert(InstructionBuilder is not null, "Internal error Instruction builder should be set in Generate already");

            EmitLocation( forInExpression );
            var function = InstructionBuilder.InsertFunction ?? throw new InternalCodeGeneratorException( "ICE: Expected block attached to a function at this point" );

            string varName = forInExpression.LoopVariable.Name;
            Alloca allocaVar = LookupVariable( varName );

            // Emit the start code first, without 'variable' in scope.
            Value? startVal;
            if(forInExpression.LoopVariable.Initializer != null)
            {
                startVal = forInExpression.LoopVariable.Initializer.Accept( this, in diBuilder );
                if(startVal is null)
                {
                    return null;
                }
            }
            else
            {
                startVal = Context.CreateConstant( 0.0 );
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
                if(forInExpression.Body.Accept( this, in diBuilder ) == null)
                {
                    return null;
                }

                Value? stepValue = forInExpression.Step.Accept( this, in diBuilder );
                if(stepValue == null)
                {
                    return null;
                }

                // Compute the end condition.
                Value? endCondition = forInExpression.Condition.Accept( this, in diBuilder );
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
        public override Value? Visit( VarInExpression varInExpression, ref readonly DIBuilder diBuilder )
        {
            ArgumentNullException.ThrowIfNull(varInExpression);

            EmitLocation( varInExpression );
            using(NamedValues.EnterScope())
            {
                EmitBranchToNewBlock( "VarInScope" );
                foreach(var localVar in varInExpression.LocalVariables)
                {
                    EmitLocation( localVar );
                    Alloca alloca = LookupVariable( localVar.Name );
                    Value initValue = Context.CreateConstant( 0.0 );
                    if(localVar.Initializer != null)
                    {
                        initValue = localVar.Initializer.Accept( this, in diBuilder ) ?? throw new CodeGeneratorException( ExpectValidExpr );
                    }

                    InstructionBuilder.Store( initValue, alloca );
                }

                EmitLocation( varInExpression );
                return varInExpression.Body.Accept( this, in diBuilder);
            }
        }
        #endregion

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

        #region EmitLocation
        private void EmitLocation( IAstNode? node )
        {
            DILocalScope? scope = null;
            if( LexicalBlocks.Count > 0 )
            {
                scope = LexicalBlocks.Peek( );
            }
            else if( InstructionBuilder.InsertFunction != null && InstructionBuilder.InsertFunction.DISubProgram != null )
            {
                scope = InstructionBuilder.InsertFunction.DISubProgram;
            }

            DILocation? loc = null;
            if( scope != null )
            {
                loc = new DILocation( InstructionBuilder.Context
                                    , ( uint )( node?.Location.StartLine ?? 0 )
                                    , ( uint )( node?.Location.StartColumn ?? 0 )
                                    , scope
                                    );
            }

            InstructionBuilder.SetDebugLocation( loc );
        }
        #endregion

        #region GetOrDeclareFunction

        // Retrieves a Function for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private Function GetOrDeclareFunction(Prototype prototype, ref readonly DIBuilder diBuilder)
        {
            if(Module is null)
            {
                throw new InvalidOperationException( "ICE: Can't get or declare a function without an active module" );
            }

            if(Module.TryGetFunction( prototype.Name, out Function? function ))
            {
                return function;
            }

            // extern declarations don't get debug information
            Function retVal;
            if( prototype.IsExtern )
            {
                var llvmSignature = Context.GetFunctionType( Context.DoubleType, prototype.Parameters.Select( _ => Context.DoubleType ) );
                retVal = Module.CreateFunction( prototype.Name, llvmSignature );
            }
            else
            {
                var parameters = prototype.Parameters;

                // DICompileUnit and File are checked for null in constructor
                var debugFile = diBuilder.CreateFile( diBuilder.CompileUnit!.File!.FileName, diBuilder.CompileUnit!.File.Directory );
                var signature = Context.CreateFunctionType( in diBuilder, DoubleType!, prototype.Parameters.Select( _ => DoubleType! ) );
                var lastParamLocation = parameters.Count > 0 ? parameters[ parameters.Count - 1 ].Location : prototype.Location;

                retVal = Module.CreateFunction(in diBuilder
                                              , scope: diBuilder.CompileUnit
                                              , name: prototype.Name
                                              , linkageName: null
                                              , file: debugFile
                                              , line: ( uint )prototype.Location.StartLine
                                              , signature
                                              , isLocalToUnit: false
                                              , isDefinition: true
                                              , scopeLine: ( uint )lastParamLocation.EndLine
                                              , debugFlags: prototype.IsCompilerGenerated ? DebugInfoFlags.Artificial : DebugInfoFlags.Prototyped
                                              , isOptimized: false
                                              );
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

        #region AddDebugInfoForAlloca
        private void AddDebugInfoForAlloca( Alloca argSlot, Function function, ParameterDeclaration param, ref readonly DIBuilder diBuilder )
        {
            uint line = ( uint )param.Location.StartLine;
            uint col = ( uint )param.Location.StartColumn;

            // Keep compiler happy on null checks by asserting on expectations
            // The items were created in this file with all necessary info so
            // these properties should never be null.
            Debug.Assert( function.DISubProgram != null, "expected function with non-null DISubProgram" );
            Debug.Assert( function.DISubProgram.File != null, "expected function with a non-null DISubProgram.File" );
            Debug.Assert( InstructionBuilder.InsertBlock != null, "expected Instruction builder with non-null insertion block" );

            DILocalVariable debugVar = diBuilder.CreateArgument( scope: function.DISubProgram
                                                               , name: param.Name
                                                               , file: function.DISubProgram.File
                                                               , line
                                                               , type: DoubleType!
                                                               , alwaysPreserve: true
                                                               , debugFlags: DebugInfoFlags.None
                                                               , argNo: checked(( ushort )( param.Index + 1 )) // Debug index starts at 1!
                                                               );
            diBuilder.InsertDeclare( storage: argSlot
                                   , varInfo: debugVar
                                   , location: new DILocation( Context, line, col, function.DISubProgram )
                                   , insertAtEnd: InstructionBuilder.InsertBlock
                                   );
        }

        private void AddDebugInfoForAlloca( Alloca argSlot, Function function, LocalVariableDeclaration localVar, ref readonly DIBuilder diBuilder )
        {
            uint line = ( uint )localVar.Location.StartLine;
            uint col = ( uint )localVar.Location.StartColumn;

            // Keep compiler happy on null checks by asserting on expectations
            // The items were created in this file with all necessary info so
            // these properties should never be null.
            Debug.Assert( function.DISubProgram != null, "expected function with non-null DISubProgram" );
            Debug.Assert( function.DISubProgram.File != null, "expected function with non-null DISubProgram.File" );
            Debug.Assert( InstructionBuilder.InsertBlock != null, "expected Instruction builder with non-null insertion block" );

            DILocalVariable debugVar = diBuilder.CreateLocalVariable( scope: function.DISubProgram
                                                                    , name: localVar.Name
                                                                    , file: function.DISubProgram.File
                                                                    , line
                                                                    , type: DoubleType!
                                                                    , alwaysPreserve: false
                                                                    , debugFlags: DebugInfoFlags.None
                                                                    );
            diBuilder.InsertDeclare( storage: argSlot
                                   , varInfo: debugVar
                                   , location: new DILocation( Context, line, col, function.DISubProgram )
                                   , insertAtEnd: InstructionBuilder.InsertBlock
                                   );
        }
        #endregion

        #region PrivateMembers
        private readonly Module Module;
        private readonly DynamicRuntimeState RuntimeState;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Alloca> NamedValues = new( );
        private readonly TargetMachine TargetMachine;
        private readonly List<Function> AnonymousFunctions = [];
        private DebugBasicType? DoubleType;
        private readonly Stack<DILocalScope> LexicalBlocks = [];
        private readonly string SourcePath;
        #endregion
    }
}
