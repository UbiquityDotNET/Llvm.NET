// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

    internal readonly record struct OperatorInfo( int TokenType, OperatorKind Kind, int Precedence, bool IsBuiltIn )
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
            if(!TryGetValue( item.TokenType, out var existingItem ))
            {
                Add( item );
                return true;
            }

            if(existingItem.IsBuiltIn)
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
            if(!Items[ index ].IsBuiltIn)
            {
                base.RemoveItem( index );
            }
        }

        protected override void ClearItems( )
        {
            var builtIns = Items.Where( oi => oi.IsBuiltIn ).ToList();
            base.ClearItems();
            foreach(var builtin in builtIns)
            {
                Add( builtin );
            }
        }

        protected override void SetItem( int index, OperatorInfo item )
        {
            if(!Items[ index ].IsBuiltIn)
            {
                base.SetItem( index, item );
            }
        }
    }
}
