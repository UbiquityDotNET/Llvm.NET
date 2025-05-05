// -----------------------------------------------------------------------
// <copyright file="PrototypeCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
