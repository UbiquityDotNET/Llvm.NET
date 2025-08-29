// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.ObjectModel;

using Kaleidoscope.Grammar.AST;

using Ubiquity.NET.Extensions;

namespace Kaleidoscope.Grammar
{
    public sealed class FunctionDefinitionCollection
        : KeyedCollection<string, FunctionDefinition>
    {
        public void AddOrReplaceItem( FunctionDefinition item )
        {
            Remove( GetKeyForItem( item ) );
            Add( item );
        }

        protected override string GetKeyForItem( FunctionDefinition item ) => item.ThrowIfNull().Name;
    }
}
