// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

using Kaleidoscope.Grammar.AST;

using Ubiquity.NET.Extensions.FluentValidation;

namespace Kaleidoscope.Grammar
{
    /// <summary>Collection of <see cref="Prototype"/> keyed by it's name (<see cref="Prototype.Name"/>)</summary>
    public sealed class PrototypeCollection
        : KeyedCollection<string, Prototype>
    {
        /// <summary>Adds or replaces an existing prototype in the collection</summary>
        /// <param name="info">Prototype to add or replace</param>
        public void AddOrReplaceItem( Prototype info )
        {
            ArgumentNullException.ThrowIfNull( info );
            Remove( info.Name );
            Add( info );
        }

        /// <summary>Gets the key for a prototype (<see cref="Prototype.Name"/>)</summary>
        /// <param name="item">Prototype to get the key from</param>
        /// <returns>Key for the prototype</returns>
        protected override string GetKeyForItem( Prototype item ) => item.ThrowIfNull().Name;
    }
}
