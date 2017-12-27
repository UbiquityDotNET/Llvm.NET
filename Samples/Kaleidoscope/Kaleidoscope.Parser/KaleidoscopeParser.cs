// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;

[assembly: CLSCompliant( false )]
[assembly: SuppressMessage( "Microsoft.Naming", "CA1715", Justification = "Generated code", MessageId = "Result", Scope = "type", Target = "Kaleidoscope.Grammar.KaleidoscopeBaseVisitor`1" )]
[assembly: SuppressMessage( "Microsoft.Naming", "CA1715", Justification = "Generated code", MessageId = "Result", Scope = "type", Target = "Kaleidoscope.Grammar.IKaleidoscopeVisitor`1" )]
[assembly: SuppressMessage( "Microsoft.Naming", "CA1708", Justification = "Generated code" )]

namespace Kaleidoscope.Grammar
{
    public enum LanguageLevel
    {
        /// <summary>Chapters 2-4</summary>
        SimpleExpressions,

        /// <summary>Chapter 5</summary>
        ControlFlow,

        /// <summary>Chapter 6</summary>
        UserDefinedOperators,

        /// <summary>Chapter 7</summary>
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

        internal DynamicRuntimeState GlobalState { get; set; }

        private bool FeatureControlFlow => IsFeatureEnabled( LanguageLevel.ControlFlow );

        private bool FeatureMutableVars => IsFeatureEnabled( LanguageLevel.MutableVariables );

        private bool FeatureUserOperators => IsFeatureEnabled( LanguageLevel.UserDefinedOperators );

        private bool IsPrefixOp( IToken op ) => GlobalState.IsPrefixOp( op );

        private bool IsFeatureEnabled( LanguageLevel feature ) => LanguageLevel >= feature;

        private int GetPrecedence( IToken token )
        {
            return GlobalState.GetIndexedPrecedence( token );
        }

        private int GetNextPrecedence( IToken token )
        {
            return GlobalState.GetNextPrecedence( token );
        }
    }
}
