// <copyright file="PrototypeCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Kaleidoscope.Grammar.AST;

namespace Kaleidoscope.Grammar
{
    public class PrototypeCollection
        : KeyedCollection<string, Prototype>
    {
        public void AddOrReplaceItem( Prototype info )
        {
            Remove( info.Name );
            Add( info );
        }

        protected override string GetKeyForItem( Prototype item ) => item.Name;
    }
}
