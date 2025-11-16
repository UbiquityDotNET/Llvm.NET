// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Values;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Tests
{
    [TestClass]
    [SuppressMessage( "StyleCop.CSharp.SpacingRules", "SA1012:Opening braces should be spaced correctly", Justification = "Empty lambda" )]
    public class BasicTests
    {
        public BasicTests( TestContext testContext )
        {
            RuntimeContext = testContext;
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is not validated in these test]" )]
        public async Task Chapter2( )
        {
            // simple basic test to ensure the well-known good input script
            // parses without errors.
            using var rdr = File.OpenText( "fibi.kls" );
            var parser = new Parser( LanguageLevel.MutableVariables );

            // Create sequence of parsed AST nodes to feed the loop
            var nodes = from stmt in rdr.ToStatements( _=>{ }, RuntimeContext.CancellationToken )
                        select parser.Parse( stmt );

            // Read, Parse, Print loop
            await foreach(IAstNode node in nodes)
            {
                RuntimeContext.WriteLine( "PARSED: {0}", node );
            }
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is not validated in this test]" )]
        public async Task Chapter3( )
        {
            using var input = File.OpenText( "simpleExpressions.kls" );
            var errors = await RunBasicReplLoop( LanguageLevel.SimpleExpressions, input, ( state, writer ) => new Chapter3.CodeGenerator( state ) );
            Assert.IsEmpty( errors );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is not validated in this test]" )]
        public async Task Chapter3_5( )
        {
            using var input = File.OpenText( "simpleExpressions.kls" );
            var errors = await RunBasicReplLoop( LanguageLevel.SimpleExpressions, input, ( state, writer ) => new Chapter3_5.CodeGenerator( state ) );
            Assert.IsEmpty( errors );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is not validated in this test]" )]
        public async Task Chapter4( )
        {
            using var input = File.OpenText( "simpleExpressions.kls" );
            var errors = await RunBasicReplLoop( LanguageLevel.SimpleExpressions, input, ( state, writer ) => new Chapter4.CodeGenerator( state, writer ) );
            Assert.IsEmpty( errors );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is not validated in this test]" )]
        public async Task Chapter5( )
        {
            using var input = File.OpenText( "ControlFlow.kls" );
            var errors = await RunBasicReplLoop( LanguageLevel.ControlFlow, input, ( state, writer ) => new Chapter5.CodeGenerator( state, writer ) );
            Assert.IsEmpty( errors );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is not validated in this test]" )]
        public async Task Chapter6( )
        {
            using var input = File.OpenText( "mandel.kls" );
            var errors = await RunBasicReplLoop( LanguageLevel.UserDefinedOperators, input, ( state, writer ) => new Chapter6.CodeGenerator( state, writer ) );
            Assert.IsEmpty( errors );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is not validated in this test]" )]
        public async Task Chapter7( )
        {
            using var input = File.OpenText( "fibi.kls" );
            var errors = await RunBasicReplLoop( LanguageLevel.MutableVariables, input, ( state, writer ) => new Chapter7.CodeGenerator( state, writer ) );
            Assert.IsEmpty( errors );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is not validated in this test]" )]
        public async Task Chapter71( )
        {
            using var input = File.OpenText( "fibi.kls" );
            var errors = await RunBasicReplLoop( LanguageLevel.MutableVariables, input, ( state, writer ) => new Chapter71.CodeGenerator( state, writer ) );
            Assert.IsEmpty( errors );
        }

        [TestMethod]
        [Description( "Test of redefinition not supported with lazy JIT [output is not validated in this test]" )]
        public async Task Chapter71_with_redefinition( )
        {
            using var input = File.OpenText( "Redefinition.kls" );
            var errors = await RunBasicReplLoop(
                                   LanguageLevel.MutableVariables,
                                   functionRedefinitionIsAnError: true,
                                   input,
                                   ( state, writer ) => new Chapter71.CodeGenerator( state, writer )
                               );
            Assert.HasCount( 1, errors );
            Assert.AreEqual( (int)DiagnosticCode.RedclarationNotSupported, errors[ 0 ].Code );
        }

        private Task<ImmutableArray<ErrorNode>> RunBasicReplLoop( LanguageLevel level
                                   , TextReader input
                                   , Func<DynamicRuntimeState, TextWriter, ICodeGenerator<Value>> generatorFactory
                                   )
        {
            return RunBasicReplLoop( level, functionRedefinitionIsAnError: false, input, generatorFactory );
        }

        private async Task<ImmutableArray<ErrorNode>> RunBasicReplLoop( LanguageLevel level
                                                                      , bool functionRedefinitionIsAnError
                                                                      , TextReader input
                                                                      , Func<DynamicRuntimeState, TextWriter, ICodeGenerator<Value>> generatorFactory
                                                                      )
        {
            var parser = new Parser( level, functionRedefinitionIsAnError );
            using var outputWriter = new TestContextTextWriter( RuntimeContext );
            using var generator = generatorFactory( parser.GlobalState, outputWriter );

            // Create sequence of parsed AST RootNodes to feed the 'REPL' loop
            var replSeq = from stmt in input.ToStatements( _=>{ }, RuntimeContext.CancellationToken )
                          select parser.Parse( stmt );

            await foreach(IAstNode node in replSeq)
            {
                var errors = node.CollectErrors();
                if(errors.Length > 0)
                {
                    return errors;
                }

                var result = generator.Generate( node );

                if(result is not null)
                {
                    switch(result)
                    {
                    case ConstantFP value:
                        outputWriter.WriteLine( "Evaluated to {0}", value.Value );
                        break;

                    case Function function:
                        outputWriter.WriteLine( "Generated:\n{0}", function.ToString() );
                        break;

                    default:
                        outputWriter.WriteLine( result.ToString() );
                        break;
                    }
                }
            }

            return [];
        }

        private readonly TestContext RuntimeContext;
    }
}
