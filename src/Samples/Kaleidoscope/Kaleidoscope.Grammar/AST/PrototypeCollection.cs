// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

using Kaleidoscope.Grammar.AST;

using Ubiquity.NET.Extensions;

namespace Kaleidoscope.Grammar
{
    public sealed class PrototypeCollection
        : KeyedCollection<string, Prototype>
    {
        public void AddOrReplaceItem( Prototype info )
        {
            ArgumentNullException.ThrowIfNull( info );
            Remove( info.Name );
            Add( info );
        }

        protected override string GetKeyForItem( Prototype item ) => item.ThrowIfNull().Name;
    }
}
