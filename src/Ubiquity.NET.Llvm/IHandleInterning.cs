// -----------------------------------------------------------------------
// <copyright file="IHandleInterning.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Ubiquity.NET.Llvm
{
    internal interface IHandleInterning<THandle, TMappedType>
        : IEnumerable<TMappedType>
    {
        Context Context { get; }

        TMappedType GetOrCreateItem( THandle handle, Action<THandle>? foundHandleRelease = null );

        void Remove( THandle handle );

        void Clear( );
    }
}
