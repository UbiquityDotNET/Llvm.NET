// <copyright file="KaleidoscopeLexer.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar
{
    // partial extension to handle creating virtual tokens/Identifiers depending
    // on language feaures enabled.
    public partial class KaleidoscopeLexer
    {
        public LanguageLevel LanguageLevel { get; set; }

        private bool FeatureControlFlow => IsFeatureEnabled( LanguageLevel.ControlFlow );

        private bool FeatureMutableVars => IsFeatureEnabled( LanguageLevel.MutableVariables );

        private bool FeatureUserOperators => IsFeatureEnabled( LanguageLevel.UserDefinedOperators );

        private bool IsFeatureEnabled( LanguageLevel feature ) => LanguageLevel >= feature;
    }
}
