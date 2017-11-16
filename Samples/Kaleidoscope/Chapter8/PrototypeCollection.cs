// <copyright file="PrototypeCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope
{
    internal class PrototypeCollection
        : Dictionary<string, IReadOnlyList<string>>
    {
        public void AddOrReplaceItem( string name, IReadOnlyList<string> parameters )
        {
            Remove( name );
            Add( name, parameters );
        }
    }
}
