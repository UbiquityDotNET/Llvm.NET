// -----------------------------------------------------------------------
// <copyright file="PrototypeCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using Kaleidoscope.Grammar.AST;
using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar
{
    public class PrototypeCollection
        : KeyedCollection<string, Prototype>
    {
        public void AddOrReplaceItem( Prototype info )
        {
            info.ValidateNotNull( nameof( info ) );
            Remove( info.Name );
            Add( info );
        }

        protected override string GetKeyForItem( Prototype item ) => item.ValidateNotNull( nameof( item ) ).Name;
    }
}
