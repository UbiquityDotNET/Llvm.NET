// -----------------------------------------------------------------------
// <copyright file="ReplEngine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Llvm.Values;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Chapter6
{
    internal class ReplEngine
        : KaleidoscopeReadEvaluatePrintLoopBase<Value>
    {
        public ReplEngine( )
            : base( LanguageLevel.UserDefinedOperators )
        {
        }

        public override ICodeGenerator<Value> CreateGenerator( DynamicRuntimeState state )
        {
            return new CodeGenerator( state );
        }

        public override void ShowResults( Value resultValue )
        {
            switch( resultValue )
            {
            case ConstantFP result:
                // If the cursor is not at the beginning of a line
                // generate a new line for it
                if( Console.CursorLeft > 0 )
                {
                    Console.WriteLine( );
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
                throw new InvalidOperationException( );
            }
        }
    }
}
