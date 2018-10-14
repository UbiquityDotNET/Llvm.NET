// <copyright file="FunctionDefinitionCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Kaleidoscope.Grammar.AST;

namespace Kaleidoscope.Grammar
{
    public class FunctionDefinitionCollection
        : KeyedCollection<string, FunctionDefinition>
    {
        public void AddOrReplaceItem( FunctionDefinition item )
        {
            Remove( GetKeyForItem(item) );
            Add( item );
        }

        protected override string GetKeyForItem( FunctionDefinition item ) => item.Name;
    }
}
