// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Antlr4.Runtime;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar.ANTLR
{
    // partial class customization to the ANTLR generated class
    // This extends the individual parse tree context node types
    // so that labels in the grammar are unnecessary.
    public partial class KaleidoscopeParser
    {
        public KaleidoscopeParser( ITokenStream tokenStream, DynamicRuntimeState globalState, IParseErrorListener? errorListener, bool useDiagnosticListener = false )
            : this( tokenStream )
        {
            globalState.ValidateNotNull( nameof( globalState ) );

            GlobalState = globalState;
            if( errorListener != null )
            {
                RemoveErrorListeners( );
                AddErrorListener( new ParseErrorListenerAdapter( errorListener ) );
            }

            if( globalState.LanguageLevel >= LanguageLevel.UserDefinedOperators )
            {
                AddParseListener( new KaleidoscopeUserOperatorListener( GlobalState ) );
            }

            if( useDiagnosticListener )
            {
                AddParseListener( new DebugTraceListener( this ) );
            }

            ErrorHandler = new FailedPredicateErrorStrategy( );
        }

        /// <summary>Gets the Language level the application supports</summary>
        public LanguageLevel LanguageLevel => GlobalState.LanguageLevel;

        /// <summary>Gets or sets the dynamic state of the runtime for the parser</summary>
        /// <remarks>
        /// This provides the <see cref="LanguageLevel"/> along with the
        /// operators currently available, including user defined operators.
        /// </remarks>
        public DynamicRuntimeState GlobalState { get; set; }

        public bool FeatureControlFlow => IsFeatureEnabled( LanguageLevel.ControlFlow );

        public bool FeatureMutableVars => IsFeatureEnabled( LanguageLevel.MutableVariables );

        public bool FeatureUserOperators => IsFeatureEnabled( LanguageLevel.UserDefinedOperators );

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
