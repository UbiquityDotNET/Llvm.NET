// -----------------------------------------------------------------------
// <copyright file="ReplEngine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.Visualizers;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Chapter2
{
    internal class ReplEngine
        : KaleidoscopeReadEvaluatePrintLoopBase<IAstNode>
    {
        public ReplEngine( )
            : base( LanguageLevel.MutableVariables )
        {
        }

        public override ICodeGenerator<IAstNode> CreateGenerator( DynamicRuntimeState state )
        {
            return new CodeGenerator( );
        }

        public override void ShowResults( IAstNode resultValue )
        {
            Console.WriteLine( "PARSED: {0}", resultValue );
            var graph = resultValue.CreateGraph( );
            // use graph in a debugger, possibly save select graphs to disk for evaluation...
        }
    }
}
