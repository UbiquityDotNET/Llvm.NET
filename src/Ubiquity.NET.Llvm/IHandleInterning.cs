// -----------------------------------------------------------------------
// <copyright file="IHandleInterning.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    internal interface IHandleInterning<THandle, TMappedType>
        : IEnumerable<TMappedType>
    {
        TMappedType GetOrCreateItem( THandle handle, Action<THandle>? foundHandleRelease = null );

        void Remove( THandle handle );

        void Clear( );
    }

   internal interface IHandleInterningWithContext<THandle, TMappedType>
        : IHandleInterning<THandle, TMappedType>
   {
        Context Context { get; }
   }
}
