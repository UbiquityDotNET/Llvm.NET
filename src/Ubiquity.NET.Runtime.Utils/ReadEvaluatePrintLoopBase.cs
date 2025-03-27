// -----------------------------------------------------------------------
// <copyright file="ReadEvaluatePrintLoopBase.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ubiquity.NET.Runtime.Utils
{
    public abstract class ReadEvaluatePrintLoopBase<T>
    {
        public abstract void ShowPrompt( ReadyState state );
        public abstract void ShowResults( T resultValue );

        public IParseErrorLogger ErrorLogger { get; }

        public async Task Run( TextReader input, IParser parser, ICodeGenerator<T> generator, CancellationToken cancelToken = default)
        {
            await Run(input, DiagnosticRepresentations.None, parser, generator, cancelToken);
        }

        public async Task Run( TextReader input, DiagnosticRepresentations diagnostics, IParser parser, ICodeGenerator<T> generator, CancellationToken cancelToken = default)
        {
            ShowPrompt( ReadyState.StartExpression );

            // Create sequence of parsed AST RootNodes to feed the REPL loop
            var replSeq = from stmt in input.ToStatements( ShowPrompt, cancelToken: cancelToken )
                          let node = parser.Parse( stmt )
                          where !ErrorLogger.CheckAndShowParseErrors( node )
                          select node;

            await foreach( IAstNode node in replSeq.WithCancellation(cancelToken) )
            {
                try
                {
                    T? result = generator.Generate( node );
                    if( result is not null )
                    {
                        ShowResults( result );
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

        protected ReadEvaluatePrintLoopBase( )
            : this( new ColoredConsoleParseErrorLogger() )
        {
        }

        protected ReadEvaluatePrintLoopBase( IParseErrorLogger logger )
        {
            ErrorLogger = logger;
        }
    }
}
