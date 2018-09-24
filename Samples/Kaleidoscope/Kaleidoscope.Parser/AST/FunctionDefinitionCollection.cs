// <copyright file="FunctionDefinitionCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Kaleidoscope.Grammar
{
    public class FunctionDefinitionCollection
        : KeyedCollection<string, AST.FunctionDefinition>
    {
        public void AddOrReplaceItem( AST.FunctionDefinition item )
        {
            Remove( GetKeyForItem(item) );
            Add( item );
        }

        protected override string GetKeyForItem( AST.FunctionDefinition item ) => item.Name;
    }
}
