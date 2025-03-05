// -----------------------------------------------------------------------
// <copyright file="HandleInterningMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Abstract base class for handle interning</summary>
    /// <typeparam name="THandle">Type of the Handle wrapper for this map</typeparam>
    /// <typeparam name="TMappedType">Projected type the handles map to</typeparam>
    internal abstract class HandleInterningMap<THandle, TMappedType>
        : IHandleInterning<THandle, TMappedType>
        where THandle : notnull, IEquatable<THandle>
    {
        /// <summary>Gets or creates a wrapped type for the handle</summary>
        /// <param name="handle">LLVM handle to wrap</param>
        /// <param name="foundHandleRelease">Action to perform if the handle was already in the map [default: null]</param>
        /// <returns>A mapped type, either found in the map, or created an added to it.</returns>
        /// <remarks>
        /// The default value for <paramref name="foundHandleRelease"/> is <see langword="null"/> as the normal
        /// behavior is NOT to release the handle in anyway but instead let the found handle in the map control
        /// the lifetime of the native object. In some cases, the provided function will set the handle to an
        /// Invalid value to prevent auto release.
        /// </remarks>
        public TMappedType GetOrCreateItem( THandle handle, Action<THandle>? foundHandleRelease = null )
        {
            ArgumentNullException.ThrowIfNull(handle);
            if( HandleMap.TryGetValue( handle, out TMappedType? retVal ) )
            {
                foundHandleRelease?.Invoke( handle );
                return retVal;
            }

            retVal = ItemFactory( handle );
            HandleMap.Add( handle, retVal );

            return retVal;
        }

        /// <summary>Disposes all entries in the map and then clears all entries from it</summary>
        public void Clear( )
        {
            DisposeItems( HandleMap.Values );
            HandleMap.Clear( );
        }

        /// <summary>Removes a handle from the map and disposes it</summary>
        /// <param name="handle">Handle to remove from the map and dispose</param>
        public void Remove( THandle handle )
        {
            if( HandleMap.TryGetValue( handle, out TMappedType? item ) )
            {
                HandleMap.Remove( handle );
                DisposeItem( item );
            }
        }

        /// <summary>Gets an enumerator for all the projected types in the map</summary>
        /// <returns>Enumerator of projected types</returns>
        public IEnumerator<TMappedType> GetEnumerator( ) => HandleMap.Values.GetEnumerator( );

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        private protected HandleInterningMap( )
        {
        }

        // Extension point to allow optimized dispose of all items if available.
        // Default will dispose each item individually in a loop.
        private protected virtual void DisposeItems( ICollection<TMappedType> items )
        {
            foreach( var item in items )
            {
                DisposeItem( item );
            }
        }

        /// <summary>Dispose a single item of a projected type</summary>
        /// <param name="item">Projected item to dispose</param>
        /// <remarks>
        /// The default base implementation does nothing. If a derived map
        /// requires additional support on Dispose() then it may override
        /// the base implementation.
        /// </remarks>
        private protected virtual void DisposeItem( TMappedType item )
        {
            // intentional NOP for base implementation
        }

        /// <summary>Factory method to create new projected items from the controlling handle</summary>
        /// <param name="handle">Handle to create the item for</param>
        /// <returns>Projected type for the handle provided</returns>
        private protected abstract TMappedType ItemFactory( THandle handle );

        // internal dictionary that maps the handle value to a projected type.
        private readonly IDictionary<THandle, TMappedType> HandleMap
            = new ConcurrentDictionary<THandle, TMappedType>( EqualityComparer<THandle>.Default );
    }
}
