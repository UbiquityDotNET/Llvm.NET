// <copyright file="PrototypeCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Kaleidoscope
{
    public class PrototypeCollection
        : KeyedCollection<string, Prototype>
    {
        public void AddOrReplaceItem( Prototype info )
        {
            Remove( info.Identifier.Name );
            Add( info );
        }

        protected override string GetKeyForItem( Prototype item ) => item.Identifier.Name;
    }
}
