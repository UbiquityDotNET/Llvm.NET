// <copyright file="DynamicRuntimeManager.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Linq;
using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    /// <summary>Class to hold dynamic runtime global state</summary>
    /// <remarks>
    /// Generally speaking the parser is the wrong place to place any
    /// sort of global state. Furthermore, the actual <see cref="KaleidoscopeParser"/>
    /// is destroyed and re-created for each REPL input string to ensure
    /// there isn't any internal parsing state carry over between parses.
    /// This provides storage and support methods for the runtime global state.
    /// In particular, it maintains the language level to use and the current set
    /// of operators, including any user defined operators so the parser know how
    /// to resolve complex expressions with precednece.
    /// </remarks>
    public class DynamicRuntimeState
    {
        public DynamicRuntimeState( LanguageLevel languageLevel )
        {
            LanguageLevel = languageLevel;
        }

        /// <summary>Gets the Language level the application supports</summary>
        public LanguageLevel LanguageLevel { get; }

        /// <summary>Attempts to add a new user defined operator</summary>
        /// <param name="token">Symbol for the operator</param>
        /// <param name="kind"><see cref="OperatorKind"/> value to define the behavior of the operator</param>
        /// <param name="precedence">precedence level for the operator</param>
        /// <returns><see langword="true"/> if the operator was added and <see langword="false"/> if not</returns>
        /// <remarks>
        /// This can add or replace user defined operators, however attempts to replace a built-in operator
        /// will not replace the operator and will simply return <see langword="false"/>.
        /// </remarks>
        public bool TryAddOperator( char token, OperatorKind kind, int precedence )
        {
            switch( kind )
            {
            case OperatorKind.InfixLeftAssociative:
            case OperatorKind.InfixRightAssociative:
                return BinOpPrecedence.TryAddOrReplaceItem( new OperatorInfo( token, kind, precedence, false ) );

            case OperatorKind.PreFix:
                return UnaryOps.TryAddOrReplaceItem( new OperatorInfo( token, kind, 0, false ) );

            // case OperatorKind.None:
            default:
                throw new ArgumentException( "unknown kind", nameof( kind ) );
            }
        }

        public OperatorInfo GetBinOperatorInfo( char opChar )
        {
            if( BinOpPrecedence.TryGetValue( opChar, out var value ) )
            {
                return value;
            }

            return default;
        }

        public OperatorInfo GetBinOperatorInfo( IToken op )
        {
            if( op != null && op.Text.Length == 1 )
            {
                return GetBinOperatorInfo( op.Text[ 0 ] );
            }

            return default;
        }

        public OperatorInfo GetUnaryOperatorInfo( char opChar )
        {
            if( UnaryOps.TryGetValue( opChar, out var value ) )
            {
                return value;
            }

            return default;
        }

        public OperatorInfo GetUnaryOperatorInfo( IToken op )
        {
            if( op != null && op.Text.Length == 1 )
            {
                return GetUnaryOperatorInfo( op.Text[ 0 ] );
            }

            return default;
        }

        internal bool IsPrefixOp( IToken op )
        {
            return UnaryOps.TryGetValue( op.Text[ 0 ], out var value );
        }

        internal int GetIndexedPrecedence( IToken token )
        {
            var opInfo = GetBinOperatorInfo( token );
            return GetIndexedPrecedence( opInfo );
        }

        internal int GetIndexedPrecedence( OperatorInfo opInfo )
        {
            return opInfo.Precedence;
            /*var sortedDistingPrecedences = BinOpPrecedence.Select( o => o.Precedence ).Distinct( ).OrderBy( p => p ).ToList( );
            //return sortedDistingPrecedences.IndexOf( opInfo.Precedence );
            */
        }

        internal int GetNextPrecedence( IToken op )
        {
            var operatorInfo = GetBinOperatorInfo( op );
            int retVal = GetIndexedPrecedence( operatorInfo );
            if( retVal == 0 )
            {
                return retVal;
            }

            if( operatorInfo.Kind == OperatorKind.InfixRightAssociative || operatorInfo.Kind == OperatorKind.PreFix )
            {
                return retVal;
            }

            return retVal + 1;
        }

        private OperatorInfoCollection UnaryOps = new OperatorInfoCollection();

        private OperatorInfoCollection BinOpPrecedence = new OperatorInfoCollection()
        {
            new OperatorInfo( '<', OperatorKind.InfixLeftAssociative, 10, true),
            new OperatorInfo( '+', OperatorKind.InfixLeftAssociative, 20, true),
            new OperatorInfo( '-', OperatorKind.InfixLeftAssociative, 20, true),
            new OperatorInfo( '*', OperatorKind.InfixLeftAssociative, 40, true),
            new OperatorInfo( '/', OperatorKind.InfixLeftAssociative, 40, true),
            new OperatorInfo( '^', OperatorKind.InfixRightAssociative, 50, true),
        };
    }
}
