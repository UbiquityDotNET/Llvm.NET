// <copyright file="OperatorInfo.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Kaleidoscope.Grammar
{
    public enum OperatorKind
    {
        None,
        InfixLeftAssociative,
        InfixRightAssociative,
        PreFix
    }

    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "Equality operators make no sense for this type" )]
    public struct OperatorInfo
    {
        public OperatorInfo( int tokenType, OperatorKind kind, int precedence )
            : this( tokenType, kind, precedence, false )
        {
        }

        public OperatorInfo( int tokenType, OperatorKind kind, int precedence, bool isBuiltIn )
        {
            TokenType = tokenType;
            Kind = kind;
            Precedence = precedence;
            IsBuiltIn = isBuiltIn;
        }

        public int TokenType { get; }

        public OperatorKind Kind { get; }

        public int Precedence { get; }

        public bool IsBuiltIn { get; }
    }

    internal class OperatorInfoCollection
        : KeyedCollection<int, OperatorInfo>
    {
        public bool TryAddOrReplaceItem( OperatorInfo item )
        {
            if(!this.TryGetValue( item.TokenType, out var existingItem ))
            {
                Add( item );
                return true;
            }

            if( existingItem.IsBuiltIn )
            {
                return false;
            }

            Remove( existingItem );
            Add( existingItem );
            return true;
        }

        protected override int GetKeyForItem( OperatorInfo item ) => item.TokenType;

        protected override void RemoveItem( int index )
        {
            if( !Items[ index ].IsBuiltIn )
            {
                base.RemoveItem( index );
            }
        }

        protected override void ClearItems( )
        {
            var builtIns = Items.Where( oi => oi.IsBuiltIn ).ToList();
            base.ClearItems( );
            foreach(var builtin in builtIns )
            {
                Add( builtin );
            }
        }

        protected override void SetItem( int index, OperatorInfo item )
        {
            if( !Items[ index ].IsBuiltIn )
            {
                base.SetItem( index, item );
            }
        }
    }
}
