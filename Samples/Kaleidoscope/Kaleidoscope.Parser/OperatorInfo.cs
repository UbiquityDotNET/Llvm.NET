// <copyright file="OperatorInfo.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

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

    public struct OperatorInfo
    {
        public OperatorInfo( char literal, OperatorKind kind, int precedence )
            : this( literal, kind, precedence, false )
        {
        }

        public OperatorInfo( char literal, OperatorKind kind, int precedence, bool isBuiltIn )
        {
            Literal = literal;
            Kind = kind;
            Precedence = precedence;
            IsBuiltIn = isBuiltIn;
        }

        public char Literal { get; }

        public OperatorKind Kind { get; }

        public int Precedence { get; }

        public bool IsBuiltIn { get; }
    }

    internal class OperatorInfoCollection
        : KeyedCollection<char, OperatorInfo>
    {
        public bool TryAddOrReplaceItem( OperatorInfo item )
        {
            if(!this.TryGetValue( item.Literal, out var existingItem ))
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

        protected override char GetKeyForItem( OperatorInfo item ) => item.Literal;

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
