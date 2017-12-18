// <copyright file="HandleInterningMap.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

// interface and common base implementation are a matched pair
#pragma warning disable SA1649 // File name must match first type name

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

    internal abstract class HandleInterningMap<THandle, TMappedType>
        : IHandleInterning<THandle, TMappedType>
    {
        public Context Context { get; }

        public TMappedType GetOrCreateItem( THandle handle )
        {
            if( EqualityComparer<THandle>.Default.Equals( handle, default ) )
            {
                return default;
            }

            if( HandleMap.TryGetValue( handle, out TMappedType retVal ) )
            {
                return retVal;
            }

            retVal = ItemFactory( handle );
            HandleMap.Add( handle, retVal );
            return retVal;
        }

        public void Clear( )
        {
            DisposeItems( HandleMap.Values );
            HandleMap.Clear( );
        }

        public void Remove( THandle handle )
        {
            if( HandleMap.TryGetValue( handle, out TMappedType item ))
            {
                HandleMap.Remove( handle );
                DisposeItem( item );
            }
        }

        public IEnumerator<TMappedType> GetEnumerator( ) => HandleMap.Values.GetEnumerator( );

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        private protected HandleInterningMap( Context context )
        {
            Context = context;
        }

        // extension point to allow optimized dispose of all items if available
        // default will dispose individually
        private protected virtual void DisposeItems( ICollection<TMappedType> items )
        {
            foreach( var item in items )
            {
                DisposeItem( item );
            }
        }

        private protected virtual void DisposeItem( TMappedType item )
        {
            // intentional NOP for base implementation
        }

        private protected abstract TMappedType ItemFactory( THandle handle );

        private IDictionary<THandle, TMappedType> HandleMap
            = new ConcurrentDictionary<THandle, TMappedType>( EqualityComparer<THandle>.Default );
    }
}
