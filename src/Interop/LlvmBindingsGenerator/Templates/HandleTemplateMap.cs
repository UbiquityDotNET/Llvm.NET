// -----------------------------------------------------------------------
// <copyright file="HandleTemplateMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LlvmBindingsGenerator.Templates
{
    internal class HandleTemplateMap
        : KeyedCollection<string, IHandleCodeTemplate>
    {
        public bool TryGetValue( string name, out IHandleCodeTemplate item )
        {
            item = null;
            if( !Contains( name ) )
            {
                return false;
            }

            item = this[ name ];
            return true;
        }

        public IEnumerable<string> HandleTypeNames => from item in this select item.HandleName;

        public IEnumerable<string> DisposeFunctionNames
            => from item in this
               let ght = item as GlobalHandleTemplate
               where ght != null && !string.IsNullOrWhiteSpace( ght.HandleDisposeFunction )
               select ght.HandleDisposeFunction;

        protected override string GetKeyForItem( IHandleCodeTemplate item ) => item.HandleName;
    }
}
