// <copyright file="HandleInterningMap.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

// Internal types don't require XML docs, despite settings in stylecop.json the analyzer still
// gripes about these for interfaces...
#pragma warning disable SA1600

namespace Llvm.NET
{
    internal interface IHandleInterning<THandle, TMappedType>
        : IEnumerable<TMappedType>
    {
        Context Context { get; }

        TMappedType GetOrCreateItem( THandle handle );

        void Remove( THandle handle );

        void Clear( );
    }
}
