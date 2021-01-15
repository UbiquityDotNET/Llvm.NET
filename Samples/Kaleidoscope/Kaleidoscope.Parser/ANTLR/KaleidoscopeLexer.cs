// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeLexer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Antlr4.Runtime;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar.ANTLR
{
    // partial extension to handle creating virtual tokens/Identifiers depending
    // on language features enabled.
    public partial class KaleidoscopeLexer
    {
        public KaleidoscopeLexer( char[ ] input, LanguageLevel languageLevel, IParseErrorListener errorListener )
            : this( new AntlrInputStream( input.ValidateNotNull( nameof( input ) ), input.Length ) )
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
