// <copyright file="PrototypeCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Kaleidoscope
{
    public class PrototypeCollection
        : KeyedCollection<string, Prototype>
    {
        public void AddOrReplaceItem( Prototype info )
        {
            Remove( info.Identifier.Name );
            Add( info );
        }

// .NET Core APP 2.0 defines this but .NET Standard 2.0 and .NET 4.x do not, sigh...
#if !NETCOREAPP2_0
        public bool TryGetValue( string key, out Prototype item )
        {
            item = default;
            if( Dictionary == null )
            {
                return false;
            }

            return Dictionary.TryGetValue( key, out item );
        }
#endif

        protected override string GetKeyForItem( Prototype item ) => item.Identifier.Name;
    }
}
