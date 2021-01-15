// -----------------------------------------------------------------------
// <copyright file="BasicTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ubiquity.NET.Llvm.Values;

namespace Kaleidoscope.Tests
{
    [TestClass]
    [SuppressMessage( "StyleCop.CSharp.SpacingRules", "SA1012:Opening braces should be spaced correctly", Justification = "Empty lambda" )]
    public class BasicTests
    {
// initialized by test framework
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public TestContext TestContext { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is unvalidated in these test]" )]
        public void Chapter2( )
        {
            // simple basic test to ensure the well-known good input script
            // parses without errors.
            using var rdr = File.OpenText( "fibi.kls" );
            var parser = new Parser( LanguageLevel.MutableVariables );

            // Create sequence of parsed AST nodes to feed the loop
            var nodes = from stmt in rdr.ToStatements( _=>{ } )
                        select parser.Parse( stmt );

            // Read, Parse, Print loop
            foreach( IAstNode node in nodes )
            {
                TestContext.WriteLine( "PARSED: {0}", node );
            }
        }

        [TestMethod]
        [Description("Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is unvalidated in this test]")]
        public void Chapter3()
        {
            using var input = File.OpenText( "SimpleExpressions.kls" );
            RunBasicReplLoop( LanguageLevel.SimpleExpressions, input, ( state, writer ) => new Chapter3.CodeGenerator( state) );
        }

        [TestMethod]
        [Description("Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is unvalidated in this test]")]
        public void Chapter4()
        {
            using var input = File.OpenText( "SimpleExpressions.kls" );
            RunBasicReplLoop( LanguageLevel.SimpleExpressions, input, ( state, writer ) => new Chapter4.CodeGenerator( state, false, writer ) );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is unvalidated in this test]" )]
        public void Chapter5( )
        {
            using var input = File.OpenText( "ControlFlow.kls" );
            RunBasicReplLoop( LanguageLevel.ControlFlow, input, ( state, writer ) => new Chapter5.CodeGenerator( state, false, writer ) );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is unvalidated in this test]" )]
        public void Chapter6( )
        {
            using var input = File.OpenText( "mandel.kls" );
            RunBasicReplLoop( LanguageLevel.UserDefinedOperators, input, ( state, writer ) => new Chapter6.CodeGenerator( state, false, writer ) );
        }

        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is unvalidated in this test]" )]
        public void Chapter7( )
        {
            using var input = File.OpenText( "fibi.kls" );
            RunBasicReplLoop( LanguageLevel.MutableVariables, input, (state, writer) => new Chapter7.CodeGenerator( state, false, writer ) );
        }

#if LAZY_FUNCTION_GENERATOR_SUPPORTED
        [TestMethod]
        [Description( "Basic test of Chapter parsing and code generation to ensure it doesn't crash on well-known good input [output is unvalidated in this test]" )]
        public void Chapter71( )
        {
            using var input = File.OpenText( "fibi.kls" );
            RunBasicReplLoop( LanguageLevel.MutableVariables, input, (state, writer) => new Chapter71.CodeGenerator( state, false, writer ) );
        }
#endif

        private void RunBasicReplLoop( LanguageLevel level
                                     , TextReader input
                                     , Func<DynamicRuntimeState, TextWriter, IKaleidoscopeCodeGenerator<Value>> generatorFactory
                                     )
        {
            var parser = new Parser( level );
            using var outputWriter = new TestContextTextWriter( TestContext );
            using var generator = generatorFactory( parser.GlobalState, outputWriter );

            // Create sequence of parsed AST RootNodes to feed the 'REPL' loop
            var replSeq = from stmt in input.ToStatements( _=>{ } )
                          select parser.Parse( stmt );

            foreach( IAstNode node in replSeq )
            {
                var errors = node.CollectErrors();
                Assert.AreEqual( 0, errors.Count( ) );

                var result = generator.Generate( node );

                // Validate guarantees of OptionalValue<T>
                Assert.IsTrue( ( result.HasValue && !( result.Value is null ) ) || ( !result.HasValue && result.Value is null ) );

                if( result.HasValue )
                {
                    switch( result.Value )
                    {
                    case ConstantFP value:
                        TestContext.WriteLine( "Evaluated to {0}", value.Value );
                        break;

                    case IrFunction function:
                        TestContext.WriteLine( "Generated:\n{0}", function.ToString( ) );
                        break;

                    default:
                        TestContext.WriteLine( result.Value!.ToString( ) );
                        break;
                    }
                }
            }
        }
    }
}
