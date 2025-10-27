// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Kaleidoscope.Grammar;

using Ubiquity.NET.CommandLine;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Runtime
{
    /// <summary>REPL Loop implementation for the Kaleidoscope language</summary>
    /// <typeparam name="T">Type of values produced by the evaluation stage</typeparam>
    public abstract class ReadEvaluatePrintLoopBase<T>
        : Ubiquity.NET.Runtime.Utils.REPLBase<T>
    {
        /// <summary>Gets the Kaleidoscope language level for this instance</summary>
        public LanguageLevel LanguageFeatureLevel { get; }

        /// <summary>Gets the <see cref="TextWriter"/> to use of output from the loop</summary>
        public TextWriter Output { get; init; } = Console.Out;

        /// <inheritdoc/>
        /// <remarks>
        /// This customizes the behavior for the Kaleidoscope language. In particular,
        /// this uses '&gt;' to identify an incomplete expression while 'Ready&gt;' is
        /// used to indicate that the start of a new expression is expected.
        /// </remarks>
        public sealed override void ShowPrompt( ReadyState state )
        {
            Output.Write( state == ReadyState.StartExpression ? "Ready>" : ">" );
        }

        /// <summary>Abstract method to create a generator for the REPL</summary>
        /// <param name="state">State instance to use when generating code</param>
        /// <returns>
        /// Generator used by the REPL to generate output based on the input to the
        /// REPL engine.
        /// </returns>
        public abstract ICodeGenerator<T> CreateGenerator( DynamicRuntimeState state );

        /// <summary>Runs the REPL for this instance</summary>
        /// <param name="input"><see cref="TextReader"/> to use as the input to the REPL</param>
        /// <param name="cancelToken">Cancellation token for ASYNC activity</param>
        /// <returns><see cref="Task"/>for the REPL operation</returns>
        public async Task Run( TextReader input, CancellationToken cancelToken = default )
        {
            await Run( input, null, cancelToken );
        }

        /// <summary>Runs the REPL for this instance</summary>
        /// <param name="input"><see cref="TextReader"/> to use as the input to the REPL</param>
        /// <param name="visualizer">Visualizer to support visualization of the results</param>
        /// <param name="cancelToken">Cancellation token for ASYNC activity</param>
        /// <returns><see cref="Task"/>for the REPL operation</returns>
        public async Task Run( TextReader input, IVisualizer? visualizer, CancellationToken cancelToken = default )
        {
            var parser = new Kaleidoscope.Grammar.Parser(LanguageFeatureLevel, visualizer);
            using ICodeGenerator<T> generator = CreateGenerator( parser.GlobalState );
            await Run( input, parser, generator, cancelToken );
        }

        /// <summary>Initializes a new instance of the <see cref="ReadEvaluatePrintLoopBase{T}"/> class.</summary>
        /// <param name="level">Language level supported by this REPL instance</param>
        /// <remarks>
        /// This is protected to prevent use by anything other than a derived type.
        /// </remarks>
        protected ReadEvaluatePrintLoopBase( LanguageLevel level )
            : this(level, new ParseErrorDiagnosticAdapter(new ColoredConsoleReporter(), "KLS"))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ReadEvaluatePrintLoopBase{T}"/> class.</summary>
        /// <param name="level">Language level supported by this REPL instance</param>
        /// <param name="logger">Logger to report any issues parsing the input.</param>
        /// <remarks>
        /// This is protected to prevent use by anything other than a derived type.
        /// </remarks>
        protected ReadEvaluatePrintLoopBase( LanguageLevel level, IParseErrorReporter logger )
            : base( logger )
        {
            LanguageFeatureLevel = level;
        }
    }
}
