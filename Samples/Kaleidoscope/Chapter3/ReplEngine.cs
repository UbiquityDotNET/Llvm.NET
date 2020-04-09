// -----------------------------------------------------------------------
// <copyright file="ReplEngine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Llvm.Values;

namespace Kaleidoscope.Chapter3
{
    internal class ReplEngine
        : ReadEvaluatePrintLoopBase<Value>
    {
        public ReplEngine( )
            : base( LanguageLevel.SimpleExpressions )
        {
        }

        public override IKaleidoscopeCodeGenerator<Value> CreateGenerator( DynamicRuntimeState state )
        {
            return new CodeGenerator( state );
        }

        public override void ShowResults( Value resultValue )
        {
            switch( resultValue )
            {
            case IrFunction function:
                Console.WriteLine( "Defined function: {0}", function.Name );
                Console.WriteLine( function );
                break;

            default:
                throw new InvalidOperationException( );
            }
        }
    }
}
