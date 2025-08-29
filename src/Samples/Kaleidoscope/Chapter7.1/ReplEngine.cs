// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Llvm.Values;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Chapter71
{
    internal class ReplEngine
        : KaleidoscopeReadEvaluatePrintLoopBase<Value>
    {
        public ReplEngine( )
            : base( LanguageLevel.MutableVariables )
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
            case ConstantFP result:
                // If the cursor is not at the beginning of a line
                // generate a new line for it
                if(Console.CursorLeft > 0)
                {
                    Console.WriteLine();
                }

                Console.WriteLine( "{0}", result.Value );
                break;

            case Function function:
#if SAVE_LLVM_IR
                string safeFileName = Utilities.GetSafeFileName( function.Name );
                _ = function.ParentModule.WriteToTextFile( System.IO.Path.ChangeExtension( safeFileName, "ll" ), out string _ );
#endif
                break;

            default:
                throw new InvalidOperationException();
            }
        }
    }
}
