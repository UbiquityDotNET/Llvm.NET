// <copyright file="RuntimeCompatShim.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

#if !NETCOREAPP2_0
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kaleidoscope
{
    // compat shims for consistency - so much for the "standard" part of .NET Standard... [Sigh...]
    public static class RuntimeCompatShim
    {
        public static bool Remove<TKey, TValue>( this IDictionary<TKey,TValue> dictionary, TKey key, out TValue value )
        {
            value = default;
            if( !dictionary.TryGetValue( key, out value ) )
            {
                return false;
            }

            dictionary.Remove( key );
            return true;
        }

        public static bool TryGetValue<TKey,TValue>( this KeyedCollection<TKey,TValue> self, TKey key, out TValue item )
        {
            item = default;
            if( !self.Contains( key ) )
            {
                return false;
            }

            item = self[ key ];
            return true;
        }
    }
}
#endif
