// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Kaleidoscope.Grammar.ANTLR;

namespace Kaleidoscope.Grammar
{
    /// <summary>ANTLR4 listener to handle adding user operator definitions parsed to the runtime state for future parsing to use</summary>
    internal class KaleidoscopeUserOperatorListener
        : KaleidoscopeBaseListener
    {
        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeUserOperatorListener"/> class.</summary>
        /// <param name="state">Dynamic runtime state of the parse</param>
        public KaleidoscopeUserOperatorListener( DynamicRuntimeState state )
        {
            RuntimeState = state;
        }

        /// <summary>Handles successfully parsed function definitions</summary>
        /// <param name="context">Context parse node for the parsed function</param>
        /// <remarks>
        /// Upon successful parse of a function definition this checks if it is a user defined operator
        /// and updates the RuntimeState accordingly, if it is. This is used for support of user defined
        /// operators providing user defined precedence.
        /// </remarks>
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
