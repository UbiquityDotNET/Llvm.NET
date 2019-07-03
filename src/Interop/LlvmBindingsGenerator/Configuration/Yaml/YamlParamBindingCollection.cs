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
    internal class YamlParamBindingCollection
        : KeyedCollection<string, YamlBindingTransform>
        , IReadOnlyDictionary<string, YamlBindingTransform>
    {
        public IEnumerable<string> Keys => Items.Select( i => GetKeyForItem( i ) );

        public IEnumerable<YamlBindingTransform> Values => Items;

        public bool ContainsKey( string key )
        {
            return TryGetValue( key, out YamlBindingTransform _ );
        }

        public bool TryGetValue( string key, out YamlBindingTransform value )
        {
            value = null;
            return Dictionary != null && Dictionary.TryGetValue( key, out value );
        }

        IEnumerator<KeyValuePair<string, YamlBindingTransform>> IEnumerable<KeyValuePair<string, YamlBindingTransform>>.GetEnumerator( )
        {
            return Items.Select( i => new KeyValuePair<string, YamlBindingTransform>( GetKeyForItem( i ), i ) )
                        .GetEnumerator( );
        }

        protected override string GetKeyForItem( YamlBindingTransform item ) => item.Name;
    }
}
