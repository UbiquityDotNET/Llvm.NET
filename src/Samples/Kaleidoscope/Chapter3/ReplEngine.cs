// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Llvm.Values;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Chapter3
{
    internal class ReplEngine
        : KaleidoscopeReadEvaluatePrintLoopBase<Value>
    {
        public ReplEngine( )
            : base( LanguageLevel.SimpleExpressions )
        {
        }

        public override ICodeGenerator<Value> CreateGenerator( DynamicRuntimeState state )
        {
            return new CodeGenerator( state );
        }

        public override void ProcessResults( Value resultValue )
        {
            switch(resultValue)
            {
            case Function function:
                Console.WriteLine( "Defined function: {0}", function.Name );
                Console.WriteLine( function );
                break;

            default:
                throw new InvalidOperationException();
            }
        }
    }
}
