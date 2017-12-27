// <copyright file="KaleidoscopeLexer.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar
{
    // partial extension to handle creating virtual tokens/Identifiers depending
    // on language feaures enabled.
    public partial class KaleidoscopeLexer
    {
        public DynamicRuntimeState GlobalState { get; internal set; }

        private void DynamciallyAdjustKeywordOrIdentifier()
        {
            switch( Text )
            {
            case "if":
                if( GlobalState.LanguageLevel >= LanguageLevel.ControlFlow )
                {
                    Type = KaleidoscopeParser.IF;
                }

                break;

            case "then":
                if( GlobalState.LanguageLevel >= LanguageLevel.ControlFlow )
                {
                    Type = KaleidoscopeParser.THEN;
                }

                break;

            case "else":
                if( GlobalState.LanguageLevel >= LanguageLevel.ControlFlow )
                {
                    Type = KaleidoscopeParser.ELSE;
                }

                break;

            case "for":
                if( GlobalState.LanguageLevel >= LanguageLevel.ControlFlow )
                {
                    Type = KaleidoscopeParser.FOR;
                }

                break;

            case "in":
                if( GlobalState.LanguageLevel >= LanguageLevel.ControlFlow )
                {
                    Type = KaleidoscopeParser.IN;
                }

                break;

            case "var":
                if( GlobalState.LanguageLevel >= LanguageLevel.MutableVariables )
                {
                    Type = KaleidoscopeParser.VAR;
                }

                break;

            case "unary":
                if( GlobalState.LanguageLevel >= LanguageLevel.UserDefinedOperators )
                {
                    Type = KaleidoscopeParser.UNARY;
                }

                break;

            case "binary":
                if( GlobalState.LanguageLevel >= LanguageLevel.UserDefinedOperators )
                {
                    Type = KaleidoscopeParser.BINARY;
                }

                break;
            }
        }
    }
}
