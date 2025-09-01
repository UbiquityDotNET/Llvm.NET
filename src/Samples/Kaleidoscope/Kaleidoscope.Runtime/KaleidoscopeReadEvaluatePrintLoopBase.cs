// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;

using Ubiquity.NET.Runtime.Utils;
using Ubiquity.NET.TextUX;

namespace Kaleidoscope.Runtime
{
    public abstract class KaleidoscopeReadEvaluatePrintLoopBase<T>
        : REPLBase<T, DiagnosticCode>
    {
        public LanguageLevel LanguageFeatureLevel { get; }

        public TextWriter Output { get; init; } = Console.Out;

        public sealed override void ShowPrompt( ReadyState state )
        {
            Output.Write( state == ReadyState.StartExpression ? "Ready>" : ">" );
        }

        public abstract ICodeGenerator<T> CreateGenerator( DynamicRuntimeState state );

        public async Task Run( TextReader input, CancellationToken cancelToken = default )
        {
            await Run( input, null, cancelToken );
        }

        public async Task Run( TextReader input, IVisualizer? visualizer, CancellationToken cancelToken = default )
        {
            var parser = new Kaleidoscope.Grammar.Parser(LanguageFeatureLevel, visualizer);
            ICodeGenerator<T> generator = CreateGenerator( parser.GlobalState );
            await Run( input, parser, generator, cancelToken );
        }

        protected KaleidoscopeReadEvaluatePrintLoopBase( LanguageLevel level )
            : this(level, new ParseErrorDiagnosticAdapter<DiagnosticCode>(new ColoredConsoleReporter(), "KLS"))
        {
        }

        protected KaleidoscopeReadEvaluatePrintLoopBase( LanguageLevel level, IParseErrorReporter<DiagnosticCode> logger )
            : base( logger )
        {
            LanguageFeatureLevel = level;
        }
    }
}
