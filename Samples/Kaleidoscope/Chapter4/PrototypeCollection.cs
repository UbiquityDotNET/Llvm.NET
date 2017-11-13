// <copyright file="PrototypeCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kaleidoscope
{
    internal class PrototypeCollection
        : KeyedCollection<string, (string Name, IList<string> Parameters)>
    {
        public void AddOrReplaceItem( (string Name, IList<string> Parameters) item )
        {
            Remove( GetKeyForItem( item ) );
            Add( item );
        }

        // .NET Core APP 2.0 defines this but .NET 4.7 and NET Standard do not, sigh...
#if !NETCOREAPP2_0
        public bool TryGetValue( string key, out (string Name, IList<string> Parameters) item)
        {
            item = default;
            if( Dictionary == null )
            {
                return false;
            }

            return Dictionary.TryGetValue( key, out item );
        }
#endif

        protected override string GetKeyForItem( (string Name, IList<string> Parameters) item ) => item.Name;
    }
}
