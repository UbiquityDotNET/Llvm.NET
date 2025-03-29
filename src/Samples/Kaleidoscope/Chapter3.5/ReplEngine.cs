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

namespace Kaleidoscope.Chapter3_5
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

        public override void ShowResults( Value resultValue )
        {
            switch( resultValue )
            {
            case Function function:
                Console.WriteLine( "Defined function: {0}", function.Name );
                Console.WriteLine( function.ParentModule.WriteToString() );
                break;

            default:
                throw new InvalidOperationException( );
            }
        }
    }
}
