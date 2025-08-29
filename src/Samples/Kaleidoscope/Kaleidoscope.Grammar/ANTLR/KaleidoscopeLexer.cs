// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Antlr4.Runtime;

using Ubiquity.NET.ANTLR.Utils;
using Ubiquity.NET.Extensions;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.ANTLR
{
    // partial extension to handle creating virtual tokens/Identifiers depending
    // on language features enabled.
    internal partial class KaleidoscopeLexer
    {
        public KaleidoscopeLexer( char[] input, LanguageLevel languageLevel, IParseErrorListener errorListener )
            : this( new AntlrInputStream( input.ThrowIfNull(), input.Length ) )
        {
            LanguageLevel = languageLevel;
            AddErrorListener( new ParseErrorListenerAdapter( errorListener ) );
        }

        public LanguageLevel LanguageLevel { get; }

        private bool FeatureControlFlow => IsFeatureEnabled( LanguageLevel.ControlFlow );

        private bool FeatureMutableVars => IsFeatureEnabled( LanguageLevel.MutableVariables );

        private bool FeatureUserOperators => IsFeatureEnabled( LanguageLevel.UserDefinedOperators );

        private bool IsFeatureEnabled( LanguageLevel feature ) => LanguageLevel >= feature;
    }
}
