// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
using Kaleidoscope.Grammar.ANTLR;

namespace Kaleidoscope.Grammar
{
    // Parse listener to handle updating runtime state upon successfully parsing a user defined
    // operator definition.
    internal class KaleidoscopeUserOperatorListener
        : KaleidoscopeBaseListener
    {
        public KaleidoscopeUserOperatorListener( DynamicRuntimeState state )
        {
            RuntimeState = state;
        }

        // upon successful parse of a function definition check if it is a user defined operator
        // and update the RuntimeState accordingly, if it is.
        public override void ExitFunctionDefinition( KaleidoscopeParser.FunctionDefinitionContext context )
        {
            switch(context.Signature)
            {
            case KaleidoscopeParser.UnaryPrototypeContext unaryProto:
                RuntimeState.TryAddOperator( unaryProto.OpToken, OperatorKind.PreFix, 0 );
                break;

            case KaleidoscopeParser.BinaryPrototypeContext binaryProto:
                RuntimeState.TryAddOperator( binaryProto.OpToken, OperatorKind.InfixLeftAssociative, binaryProto.Precedence );
                break;

            default:
                base.ExitFunctionDefinition( context );
                break;
            }
        }

        private readonly DynamicRuntimeState RuntimeState;
    }
}
