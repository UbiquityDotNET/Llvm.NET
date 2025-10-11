// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.ObjectModel;

using Kaleidoscope.Grammar.AST;

using Ubiquity.NET.Extensions;

namespace Kaleidoscope.Grammar
{
    /// <summary>Collection of function definitions keyed by the name of the function</summary>
    public sealed class FunctionDefinitionCollection
        : KeyedCollection<string, FunctionDefinition>
    {
        /// <summary>Adds or replaces a function in the collection</summary>
        /// <param name="item">function definition to add or replace in the collection</param>
        public void AddOrReplaceItem( FunctionDefinition item )
        {
            Remove( GetKeyForItem( item ) );
            Add( item );
        }

        /// <summary>Gets the key for this item (<see cref="FunctionDefinition.Name"/>)</summary>
        /// <param name="item">Definition to get the key for</param>
        /// <returns>Key for the entry</returns>
        protected override string GetKeyForItem( FunctionDefinition item ) => item.ThrowIfNull().Name;
    }
}
