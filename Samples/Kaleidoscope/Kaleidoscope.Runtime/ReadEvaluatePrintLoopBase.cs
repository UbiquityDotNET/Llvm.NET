// -----------------------------------------------------------------------
// <copyright file="ReadEvaluatePrintLoopBase.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;

namespace Kaleidoscope.Runtime
{
    public abstract class ReadEvaluatePrintLoopBase<T>
    {
        public LanguageLevel LanguageFeatureLevel { get; }

        public virtual void ShowPrompt( ReadyState state )
        {
            Console.Write( state == ReadyState.StartExpression ? "Ready>" : ">" );
        }

        public abstract IKaleidoscopeCodeGenerator<T> CreateGenerator( DynamicRuntimeState state );

        public IParseErrorLogger ErrorLogger { get; }

        public abstract void ShowResults( T resultValue );

        public void Run( TextReader input )
        {
            var parser = new Parser( LanguageFeatureLevel );
            using var generator = CreateGenerator( parser.GlobalState );

            ShowPrompt( ReadyState.StartExpression );

            // Create sequence of parsed AST RootNodes to feed the REPL loop
            var replSeq = from stmt in input.ToStatements( ShowPrompt )
                          select parser.Parse( stmt );

            foreach( IAstNode node in replSeq )
            {
                try
                {
                    if( !ErrorLogger.CheckAndShowParseErrors( node ) )
                    {
                        var result = generator.Generate( node );

                        if( result.HasValue )
                        {
                            ShowResults( result.Value! );
                        }
                    }
                }
                catch( CodeGeneratorException ex )
                {
                    // This is an internal error that is not recoverable.
                    // Show the error and stop additional processing
                    ErrorLogger.ShowError( ex.ToString( ) );
                    break;
                }
            }
        }

        protected ReadEvaluatePrintLoopBase( LanguageLevel level )
            : this( level, new ColoredConsoleParseErrorLogger() )
        {
        }

        protected ReadEvaluatePrintLoopBase( LanguageLevel level, IParseErrorLogger logger )
        {
            LanguageFeatureLevel = level;
            ErrorLogger = logger;
        }
    }
}
