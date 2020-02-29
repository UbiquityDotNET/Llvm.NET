// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeUserOperatorListener.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Kaleidoscope.Grammar;

namespace Kaleidoscope.Runtime
{
    // Parse listener to handle updating runtime state on successfully parsing a user defined
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
            switch( context.Signature )
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
