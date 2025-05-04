// -----------------------------------------------------------------------
// <copyright file="OperatorInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
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

    internal readonly record struct OperatorInfo(int TokenType, OperatorKind Kind, int Precedence, bool IsBuiltIn)
    {
        public OperatorInfo( int tokenType, OperatorKind kind, int precedence )
            : this( tokenType, kind, precedence, false )
        {
        }
    }

    internal class OperatorInfoCollection
        : KeyedCollection<int, OperatorInfo>
    {
        public bool TryAddOrReplaceItem( OperatorInfo item )
        {
            if( !TryGetValue( item.TokenType, out var existingItem ) )
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
            foreach( var builtin in builtIns )
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
