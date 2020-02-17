// -----------------------------------------------------------------------
// <copyright file="RuntimeCompatShim.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETCOREAPP2_0
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ubiquity.ArgValidators;

namespace Kaleidoscope
{
    // compatibility shims for consistency - so much for the "standard" part of .NET Standard... [Sigh...]
    public static class RuntimeCompatShim
    {
        public static bool Remove<TKey, TValue>( [ValidatedNotNull] this IDictionary<TKey,TValue> self, TKey key, out TValue value )
        {
            self.ValidateNotNull( nameof( self ) );
            if( !self.TryGetValue( key, out value ) )
            {
                return false;
            }

            self.Remove( key );
            return true;
        }

        public static bool TryGetValue<TKey,TValue>( [ValidatedNotNull] this KeyedCollection<TKey,TValue> self, TKey key, out TValue item )
        {
            self.ValidateNotNull( nameof( self ) );
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
