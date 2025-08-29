// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Antlr4.Runtime;

using static Kaleidoscope.Grammar.ANTLR.KaleidoscopeLexer;

namespace Kaleidoscope.Grammar
{
    /// <summary>Class to hold dynamic runtime global state</summary>
    /// <remarks>
    /// <para>Generally speaking the parser is the wrong place to store any
    /// sort of global state. Furthermore, the actual underlying ANTLR parser
    /// is destroyed and re-created for each REPL input string to ensure
    /// there isn't any internal parsing state carry over between parses of partial
    /// input text. Thus, this class serves to maintain state for parsing and the
    /// current state of the runtime as they are tightly linked in an interactive
    /// language like Kaleidoscope.
    /// </para>
    /// <para>
    /// This provides storage and support methods for the runtime global state.
    /// The state includes:
    /// * The language level to use for parsing
    /// * The current set of operators, including any user defined operators so the
    ///   parser knows how to resolve complex expressions with user defined operators
    ///   and precedence.
    /// * The current set of external declarations
    /// * The current set of defined functions
    /// </para>
    /// </remarks>
    public class DynamicRuntimeState
    {
        /// <summary>Initializes a new instance of the <see cref="DynamicRuntimeState"/> class.</summary>
        /// <param name="languageLevel">Language level supported for this instance</param>
        public DynamicRuntimeState( LanguageLevel languageLevel )
        {
            LanguageLevel = languageLevel;
        }

        /// <summary>Gets or sets the Language level the application supports</summary>
        public LanguageLevel LanguageLevel { get; set; }

        /// <summary>Gets a collection of function definitions parsed but not yet generated</summary>
        /// <remarks>
        /// This is a potentially dynamic set as parsing can add new entries. Also, when fully lazy
        /// compilation is implemented the definitions are removed when they are generated to native
        /// and a declaration takes it's place so that there is a sort of "Garbage Collection" to
        /// remove definitions when no longer needed.
        /// </remarks>
        public FunctionDefinitionCollection FunctionDefinitions { get; } = [];

        /// <summary>Gets a collection of declared functions</summary>
        public PrototypeCollection FunctionDeclarations { get; } = [];

        /// <summary>Generates a new unique name for an anonymous function</summary>
        /// <returns>Name for an anonymous function</returns>
        public string GenerateAnonymousName( )
        {
            return $"anon_expr_{AnonymousNameIndex++}";
        }

        /// <summary>Attempts to add a new user defined operator</summary>
        /// <param name="token">Symbol for the operator</param>
        /// <param name="kind"><see cref="OperatorKind"/> value to define the behavior of the operator</param>
        /// <param name="precedence">precedence level for the operator</param>
        /// <returns><see langword="true"/> if the operator was added and <see langword="false"/> if not</returns>
        /// <remarks>
        /// This can add or replace user defined operators, however attempts to replace a built-in operator
        /// will not replace the operator and will simply return <see langword="false"/>.
        /// </remarks>
        internal bool TryAddOperator( IToken token, OperatorKind kind, int precedence )
        {
            ArgumentNullException.ThrowIfNull( token );
            return TryAddOperator( token.Type, kind, precedence );
        }

        /// <summary>Gets the binary operator information for a given token type</summary>
        /// <param name="tokenType">Operator token type</param>
        /// <returns>Operator info for the operator or default if not found</returns>
        internal OperatorInfo GetBinOperatorInfo( int tokenType )
        {
            return BinOpPrecedence.TryGetValue( tokenType, out var value ) ? value : (default);
        }

        /// <summary>Gets the unary operator information for a given token type</summary>
        /// <param name="tokenType">Operator token type</param>
        /// <returns>Operator info for the operator or default if not found</returns>
        internal OperatorInfo GetUnaryOperatorInfo( int tokenType )
        {
            return UnaryOps.TryGetValue( tokenType, out var value ) ? value : (default);
        }

        internal bool IsPrefixOp( int tokenType )
        {
            return UnaryOps.TryGetValue( tokenType, out _ );
        }

        internal int GetPrecedence( int tokenType ) => GetBinOperatorInfo( tokenType ).Precedence;

        internal int GetNextPrecedence( int tokenType )
        {
            var operatorInfo = GetBinOperatorInfo( tokenType );
            int retVal = operatorInfo.Precedence;
            return operatorInfo.Kind == OperatorKind.InfixRightAssociative || operatorInfo.Kind == OperatorKind.PreFix ? retVal : retVal + 1;
        }

        private bool TryAddOperator( int tokenType, OperatorKind kind, int precedence )
        {
            // internally operators are stored as token type integers to accommodate
            // simpler condition checks and switching on operator types in code generation
            return kind switch
            {
                OperatorKind.InfixLeftAssociative or
                OperatorKind.InfixRightAssociative => BinOpPrecedence.TryAddOrReplaceItem( new OperatorInfo( tokenType, kind, precedence, false ) ),
                OperatorKind.PreFix => UnaryOps.TryAddOrReplaceItem( new OperatorInfo( tokenType, kind, 0, false ) ),
                _ => throw new ArgumentException( "unknown kind", nameof( kind ) ),
            };
        }

        private readonly OperatorInfoCollection UnaryOps = [];

        private readonly OperatorInfoCollection BinOpPrecedence =
        [
            new( LEFTANGLE, OperatorKind.InfixLeftAssociative, 10, true),
            new( PLUS,      OperatorKind.InfixLeftAssociative, 20, true),
            new( MINUS,     OperatorKind.InfixLeftAssociative, 20, true),
            new( ASTERISK,  OperatorKind.InfixLeftAssociative, 40, true),
            new( SLASH,     OperatorKind.InfixLeftAssociative, 40, true),
            new( CARET,     OperatorKind.InfixRightAssociative, 50, true),
            new( ASSIGN,    OperatorKind.InfixRightAssociative, 2, true)
        ];

        private int AnonymousNameIndex;
    }
}
