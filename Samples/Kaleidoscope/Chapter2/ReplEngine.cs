// -----------------------------------------------------------------------
// <copyright file="ReplEngine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

namespace Kaleidoscope.Chapter2
{
    internal class ReplEngine
        : ReadEvaluatePrintLoopBase<IAstNode>
    {
        public ReplEngine( )
            : base( LanguageLevel.MutableVariables )
        {
        }

        public override IKaleidoscopeCodeGenerator<IAstNode> CreateGenerator( DynamicRuntimeState state )
        {
            return new CodeGenerator( );
        }

        public override void ShowResults( IAstNode resultValue )
        {
            Console.WriteLine( "PARSED: {0}", resultValue );
            var graph = resultValue.CreateGraph( );
        }
    }
}
