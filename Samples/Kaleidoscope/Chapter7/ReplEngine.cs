﻿// -----------------------------------------------------------------------
// <copyright file="ReplEngine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Llvm.Values;

namespace Kaleidoscope.Chapter7
{
    internal class ReplEngine
        : ReadEvaluatePrintLoopBase<Value>
    {
        public ReplEngine( )
            : base( LanguageLevel.MutableVariables )
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
            case ConstantFP result:
                if( Console.CursorLeft > 0 )
                {
                    Console.WriteLine( );
                }

                Console.WriteLine( "Evaluated to {0}", result.Value );
                break;

            case IrFunction function:
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
