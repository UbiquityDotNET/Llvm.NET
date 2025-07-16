// -----------------------------------------------------------------------
// <copyright file="ReplEngine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Kaleidoscope.Grammar;
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
            return new CodeGenerator();
        }

        public override void ProcessResults( IAstNode resultValue )
        {
            Console.WriteLine( "PARSED: {0}", resultValue );
        }
    }
}
