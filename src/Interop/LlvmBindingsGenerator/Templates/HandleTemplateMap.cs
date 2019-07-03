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
        , IReadOnlyDictionary<string, IHandleCodeTemplate>
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

        public IEnumerable<string> DisposeFunctionNames
            => from item in Items
               let ght = item as GlobalHandleTemplate
               where ght != null && !string.IsNullOrWhiteSpace( ght.HandleDisposeFunction )
               select ght.HandleDisposeFunction;

        public IEnumerable<string> Keys => Items.Select( GetKeyForItem );

        public IEnumerable<IHandleCodeTemplate> Values => Items;

        public bool ContainsKey( string key ) => TryGetValue( key, out IHandleCodeTemplate _ );

        IEnumerator<KeyValuePair<string, IHandleCodeTemplate>> IEnumerable<KeyValuePair<string, IHandleCodeTemplate>>.GetEnumerator( )
        {
            return Items.Select( i => new KeyValuePair<string, IHandleCodeTemplate>( GetKeyForItem( i ), i ) )
                        .GetEnumerator( );
        }

        protected override string GetKeyForItem( IHandleCodeTemplate item ) => item.HandleName;
    }
}
