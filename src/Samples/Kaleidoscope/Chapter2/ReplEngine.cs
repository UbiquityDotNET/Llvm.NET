// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
