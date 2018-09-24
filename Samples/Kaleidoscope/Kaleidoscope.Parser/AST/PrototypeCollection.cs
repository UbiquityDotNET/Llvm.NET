// <copyright file="PrototypeCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Kaleidoscope.Grammar
{
    public class PrototypeCollection
        : KeyedCollection<string, AST.Prototype>
    {
        public void AddOrReplaceItem( AST.Prototype info )
        {
            Remove( info.Name );
            Add( info );
        }

        protected override string GetKeyForItem( AST.Prototype item ) => item.Name;
    }
}
