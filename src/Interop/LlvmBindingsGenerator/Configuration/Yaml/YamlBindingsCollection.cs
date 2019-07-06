// -----------------------------------------------------------------------
// <copyright file="YamlBindingsCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LlvmBindingsGenerator.Configuration
{
    internal class YamlBindingsCollection
        : KeyedCollection<string, YamlFunctionBinding>
        , IReadOnlyDictionary<string, YamlFunctionBinding>
    {
        public IEnumerable<string> Keys => Items.Select( i => GetKeyForItem( i ) );

        public IEnumerable<YamlFunctionBinding> Values => Items;

        public bool ContainsKey( string key )
        {
            return TryGetValue( key, out YamlFunctionBinding _ );
        }

        public bool TryGetValue( string key, out YamlFunctionBinding value )
        {
            value = null;
            return Dictionary != null && Dictionary.TryGetValue( key, out value );
        }

        IEnumerator<KeyValuePair<string, YamlFunctionBinding>> IEnumerable<KeyValuePair<string, YamlFunctionBinding>>.GetEnumerator( )
        {
            return Items.Select( i => new KeyValuePair<string, YamlFunctionBinding>( GetKeyForItem( i ), i ) )
                        .GetEnumerator( );
        }

        protected override string GetKeyForItem( YamlFunctionBinding item ) => item.Name;
    }
}
