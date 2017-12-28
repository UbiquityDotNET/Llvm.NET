// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    /// <summary>Language level supported</summary>
    public enum LanguageLevel
    {
        /// <summary>Chapters 2-4 Simple Expressions</summary>
        SimpleExpressions,

        /// <summary>Chapter 5 - Control Flow</summary>
        ControlFlow,

        /// <summary>Chapter 6 - User defined operators </summary>
        UserDefinedOperators,

        /// <summary>Chapter 7 - Mutable Variables </summary>
        MutableVariables,
    }

    // partial class customization to the ANTLR generated class
    // This extends the individual parse tree context node types
    // so that labels in the grammar are unnecessary.
    public partial class KaleidoscopeParser
        : Parser
    {
        /// <summary>Gets the Language level the application supports</summary>
        public LanguageLevel LanguageLevel => GlobalState.LanguageLevel;

        /// <summary>Gets or sets the dynamic state of the runtime for the parser</summary>
        /// <remarks>
        /// This provides the <see cref="LanguageLevel"/> along with the
        /// operators currently available, including user defined operators.
        /// </remarks>
        public DynamicRuntimeState GlobalState { get; set; }

        private bool FeatureControlFlow => IsFeatureEnabled( LanguageLevel.ControlFlow );

        private bool FeatureMutableVars => IsFeatureEnabled( LanguageLevel.MutableVariables );

        private bool FeatureUserOperators => IsFeatureEnabled( LanguageLevel.UserDefinedOperators );

        private bool IsFeatureEnabled( LanguageLevel feature ) => LanguageLevel >= feature;

        private bool IsPrefixOp( ) => GlobalState.IsPrefixOp( _input.Lt( 1 ).Type );

        private int GetPrecedence( )
        {
            return GlobalState.GetPrecedence( _input.Lt( 1 ).Type );
        }

        private int GetNextPrecedence( )
        {
            return GlobalState.GetNextPrecedence( _input.Lt( -1 ).Type );
        }
    }
}
