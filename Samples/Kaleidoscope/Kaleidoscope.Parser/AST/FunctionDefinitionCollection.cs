﻿// -----------------------------------------------------------------------
// <copyright file="FunctionDefinitionCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;

using Kaleidoscope.Grammar.AST;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar
{
    public class FunctionDefinitionCollection
        : KeyedCollection<string, FunctionDefinition>
    {
        public void AddOrReplaceItem( FunctionDefinition item )
        {
            Remove( GetKeyForItem( item ) );
            Add( item );
        }

        protected override string GetKeyForItem( FunctionDefinition item ) => item.ValidateNotNull( nameof( item ) ).Name;
    }
}
