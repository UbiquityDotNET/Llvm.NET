// -----------------------------------------------------------------------
// <copyright file="REPLBase.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Common core implementation of a Read, Evaluate, Print Loop (REPL)</summary>
    /// <typeparam name="T">Type of values produce by the evaluation stage</typeparam>
    public abstract class REPLBase<T>
    {
        /// <summary>Shows a prompt appropriate for the runtime and current state</summary>
        /// <param name="state">Ready state for the REPL</param>
        public abstract void ShowPrompt( ReadyState state );

        /// <summary>Show or otherwise process the results of an evaluation</summary>
        /// <param name="resultValue">Evaluated result value</param>
        public abstract void ShowResults( T resultValue );

        /// <summary>Gets the error logger to use for logging any parse errors</summary>
        public IParseErrorReporter ErrorLogger { get; }

        /// <summary>Asynchronously runs the REPL loop on the input reader</summary>
        /// <param name="input">Reader to process the input for</param>
        /// <param name="parser">Parser to process the input for</param>
        /// <param name="generator">Result generator to transform nodes parsed into the evaluated result</param>
        /// <param name="cancelToken">Cancellation token to allow cancellation of the REPL loop</param>
        /// <returns>Async task for the operation</returns>
        public async Task Run( TextReader input, IParser parser, ICodeGenerator<T> generator, CancellationToken cancelToken = default )
        {
            ShowPrompt( ReadyState.StartExpression );

            // Create sequence of parsed AST RootNodes to feed the REPL loop
            var replSeq = from stmt in input.ToStatements( ShowPrompt, cancelToken: cancelToken )
                          let node = parser.Parse( stmt )
                          where !ErrorLogger.CheckAndReportParseErrors( node )
                          select node;

            await foreach(IAstNode node in replSeq.WithCancellation( cancelToken ))
            {
                try
                {
                    T? result = generator.Generate( node );
                    if(result is not null)
                    {
                        ShowResults( result );
                    }
                }
                catch(CodeGeneratorException ex)
                {
                    // This is an internal error that is not recoverable.
                    // Show the error and stop additional processing
                    ErrorLogger.ReportError( ex.ToString() );
                    break;
                }
            }
        }

        /// <summary>Initializes a new instance of the <see cref="REPLBase{T}"/> class</summary>
        protected REPLBase( )
            : this( new ColoredConsoleParseErrorReporter() )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="REPLBase{T}"/> class</summary>
        /// <param name="logger">Logger to use for reporting any errors during parse</param>
        protected REPLBase( IParseErrorReporter logger )
        {
            ErrorLogger = logger;
        }
    }
}
