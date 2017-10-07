﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Llvm.NET.Values
{
    // re-usable implementation of IAttributeDictionary for containers that implement IAttributeAccessor
    // This uses the lowlevel methods of IAttributeAccessor to abastract out the differences in the
    // LLVM-C API for attributes on CallSites vs. Functions
    internal class ValueAttributeDictionary
        : IAttributeDictionary
    {
        internal ValueAttributeDictionary( IAttributeAccessor container, Func<Function> functionFetcher )
        {
            Container = container;
            FunctionFetcher = functionFetcher;
        }

        public Context Context => Container.Context;

        public ICollection<AttributeValue> this[ FunctionAttributeIndex key ]
        {
            get
            {
                if( !ContainsKey( key ) )
                {
                    throw new KeyNotFoundException( );
                }

                return new ValueAttributeCollection( Container, key );
            }
        }

        public IEnumerable<FunctionAttributeIndex> Keys
            => new ReadOnlyCollection<FunctionAttributeIndex>( GetValidKeys( ).ToList( ) );

        public IEnumerable<ICollection<AttributeValue>> Values
            => new ReadOnlyCollection<ICollection<AttributeValue>>( this.Select( kvp => kvp.Value ).ToList( ) );

        public int Count => GetValidKeys( ).Count( );

        public bool ContainsKey( FunctionAttributeIndex key ) => GetValidKeys( ).Any( k => k == key );

        public IEnumerator<KeyValuePair<FunctionAttributeIndex, ICollection<AttributeValue>>> GetEnumerator( )
        {
            return ( from key in GetValidKeys( )
                     select new KeyValuePair<FunctionAttributeIndex, ICollection<AttributeValue>>( key, this[ key ] )
                   ).GetEnumerator( );
        }

        public bool TryGetValue( FunctionAttributeIndex key, out ICollection<AttributeValue> value )
        {
            value = null;
            if( ContainsKey( key ) )
            {
                return false;
            }

            value = new ValueAttributeCollection( Container, key );
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        private IEnumerable<FunctionAttributeIndex> GetValidKeys()
        {
            var endIndex = FunctionAttributeIndex.Parameter0 + FunctionFetcher().Parameters.Count;
            for( var index = FunctionAttributeIndex.Function; index < endIndex; ++index )
            {
                if( Container.GetAttributeCountAtIndex( index ) > 0 )
                {
                    yield return index;
                }
            }
        }

        private readonly Func<Function> FunctionFetcher;
        private readonly IAttributeAccessor Container;
    }
}
